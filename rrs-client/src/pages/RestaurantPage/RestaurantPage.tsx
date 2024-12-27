import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getRestaurantById, updateRestaurant, uploadRestaurantLogo } from "../../services/RestaurantService";
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
import RestaurantDetails from "../../components/restaurantDetails/RestaurantDetails";
import RestaurantTables from "../../components/restaurantTables/RestaurantTables";
import ManagerActions from "../../components/managerActions/ManagerActions";


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
  const [isLogoModalOpen, setIsLogoModalOpen] = useState(false);
  const [logoFile, setLogoFile] = useState<File | null>(null);


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
        console.error("Failed to fetch restaurant details or tables:", error);
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

  const handleLogoChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      setLogoFile(event.target.files[0]);
    }
  };

  const handleLogoUpload = async () => {
    if (!id || !logoFile) return;

    try {
      const formData = new FormData();
      formData.append("file", logoFile);

      await uploadRestaurantLogo(id, formData);
      notifySuccess("Logo updated successfully!");
      setIsLogoModalOpen(false);

      const updatedRestaurant = await getRestaurantById(id);
      setRestaurant(updatedRestaurant);
    } catch (error) {
      notifyError("Failed to upload logo.");
    }
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
      <RestaurantDetails restaurant={restaurant} />
      <RestaurantTables tables={tables} />
      {isManager && (
        <ManagerActions
          onAddTables={() => setIsAddTablesModalOpen(true)}
          onUpdateRestaurant={() => setIsUpdateModalOpen(true)}
          onChangeLogo={() => setIsLogoModalOpen(true)}
        />
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

      <Modal
        title="Change Logo"
        isOpen={isLogoModalOpen}
        onClose={() => setIsLogoModalOpen(false)}
      >
        <div>
          <input type="file" accept="image/*" onChange={handleLogoChange} />
          <button onClick={handleLogoUpload}>Upload</button>
        </div>
      </Modal>
    </div>
  );
};

export default RestaurantPage;
