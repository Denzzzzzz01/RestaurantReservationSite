import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getRestaurantById, updateRestaurant } from "../../services/RestaurantService";
import { DetailedRestaurantDto } from "../../models/DetailedRestaurantDto";
import { jwtDecode } from "jwt-decode";
import { useAuth } from "../../context/useAuth";
import { TableDto } from "../../models/TableDto";
import { createTables, getRestaurantTables } from "../../services/TableService";
import AddTablesForm from "../../components/addTablesForm/AddTablesForm";
import Modal from "../../components/modal/Modal";
import UpdateRestaurantForm from "../../components/updateRestaurantModal/updateRestaurantModal";
import { RestaurantUpdateDto } from "../../models/RestaurantUpdateDto";
import { notifyError, notifySuccess, toastPromise } from "../../utils/toastUtils";


const RestaurantPage = () => {
  const { id } = useParams<{ id: string }>();
  const [restaurant, setRestaurant] = useState<DetailedRestaurantDto | null>(null);
  const [tables, setTables] = useState<TableDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isManager, setIsManager] = useState(false); 
  const { token } = useAuth();
  const [isAddTablesModalOpen, setIsAddTablesModalOpen] = useState(false);
  const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);


  useEffect(() => {
    const fetchRestaurantData = async () => {
      try {
        if (id) {
          const [restaurantData, tablesData] = await Promise.all([
            getRestaurantById(id),
            getRestaurantTables(id),
          ]);
          setRestaurant(restaurantData);
          setTables(tablesData);
        }
      } catch (error) {
        notifyError("Failed to fetch restaurant details or tables.");
        setError("Failed to fetch restaurant details or tables.");
      } finally {
        setLoading(false);
      }
    };

    fetchRestaurantData();
  }, [id]);
  

  const handleAddTables = async (
    numberOfTables: number,
    tableCapacity: number,
    description: string
  ) => {
    if (!id) return;

    try {
      await createTables(id, { numberOfTables, tableCapacity, description });
      setIsAddTablesModalOpen(false);
      const updatedTables = await getRestaurantTables(id); 
      setTables(updatedTables);
      notifySuccess("Tables added successfully.");
    } catch (error) {
      notifyError("Error adding tables.");
      console.error("Error adding tables:", error);
    }
  };
  


  // const handleUpdateRestaurant = async (updatedData: RestaurantUpdateDto) => {
  //   if (!id) return;

  //   try {
  //     await updateRestaurant(id, updatedData);
  //     const updatedRestaurant = await getRestaurantById(id);
  //     setRestaurant(updatedRestaurant);
  //     setIsUpdateModalOpen(false);
  //     notifySuccess("Restaurant updated successfully.");
  //   } catch (error) {
  //     notifyError("Failed to update restaurant.");
  //     console.error("Error updating restaurant:", error);
  //   }
  // };
  const handleUpdateRestaurant = async (updatedData: RestaurantUpdateDto) => {
    if (!id) return;
  
    await toastPromise(
      updateRestaurant(id, updatedData)
        .then(async () => {
          const updatedRestaurant = await getRestaurantById(id);
          setRestaurant(updatedRestaurant);
          setIsUpdateModalOpen(false);
        }),
      "Updating restaurant...",
      "Restaurant updated successfully!",
      "Failed to update restaurant."
    );
  };
  

  useEffect(() => {
    if (token) {
      const decodedToken = jwtDecode<{ role?: string[] }>(token); 
      const roles = decodedToken?.role || [];
      const isRestaurantManager = roles.includes("RestaurantManager"); 
      setIsManager(isRestaurantManager);
    } else {
      setIsManager(false);
    }
  }, [token]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  if (!restaurant) {
    return <div>Restaurant not found</div>;
  }

  return (
    <div>
      <h1>{restaurant.name}</h1>
      <p>
        Address: {restaurant.address.street}, {restaurant.address.city},{" "}
        {restaurant.address.country}
      </p>
      <p>Phone: {restaurant.phoneNumber}</p>
      <p>Website: {restaurant.website || "N/A"}</p>
      <p>
        Opening Hours: {restaurant.openingHour} - {restaurant.closingHour}
      </p>
      <h3>Managers:</h3>
      <ul>
        {restaurant.manageres.map((manager) => (
          <li key={manager.appUser.id}>{manager.appUser.userName}</li>
        ))}
      </ul>
      {isManager && (
        <div style={{ marginTop: "20px" }}>
          <Link to={`/restaurant-reservations/${restaurant.id}`} className="btn btn-primary">
            View Reservations
          </Link>
        </div>
      )}

      <h3>Tables:</h3>
      {tables.length > 0 ? (
        <ul>
          {tables.map((table) => (
            <li key={table.id}>
              <p>Table Number: {table.tableNumber}</p>
              <p>Capacity: {table.capacity}</p>
              <p>Description: {table.description}</p>
              <p>Status: {table.isAvailable ? "Available" : "Unavailable"}</p>
            </li>
          ))}
        </ul>
      ) : (
        <p>No tables available.</p>
      )}

      <button onClick={() => setIsAddTablesModalOpen(true)}>Add Tables</button>

      {isManager && (
        <button onClick={() => setIsUpdateModalOpen(true)}>
          Update Restaurant
        </button>
      )}

      <Modal
        title="Add Tables"
        isOpen={isAddTablesModalOpen}
        onClose={() => setIsAddTablesModalOpen(false)}
      >
        <AddTablesForm onSubmit={handleAddTables} />
      </Modal>

      <Modal
            title="Update Restaurant"
            isOpen={isUpdateModalOpen}
            onClose={() => setIsUpdateModalOpen(false)}
          >
            {restaurant && (
              <UpdateRestaurantForm
                initialData={restaurant}
                onSubmit={handleUpdateRestaurant}
                onClose={() => setIsUpdateModalOpen(false)}
              />
            )}
          </Modal>
    </div>
  );
};

export default RestaurantPage;
