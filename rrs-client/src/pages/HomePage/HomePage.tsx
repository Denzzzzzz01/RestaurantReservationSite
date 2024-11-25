import React, { useEffect, useState } from "react";
import { RestaurantDto } from "../../models/RestaurantDto";
import { getRestaurants } from "../../services/RestaurantService";
import BookTableForm from "../../components/bookTableForm/BookTableForm";
import Modal from "../../components/modal/Modal";

const HomePage: React.FC = () => {
  const [restaurants, setRestaurants] = useState<RestaurantDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);
  const [totalCount, setTotalCount] = useState<number>(0);
  const [totalPages, setTotalPages] = useState<number>(1);

  const [selectedRestaurantId, setSelectedRestaurantId] = useState<string | null>(null);

  useEffect(() => {
    const fetchRestaurants = async () => {
      setLoading(true);
      setError(null);

      try {
        const data = await getRestaurants(pageNumber, pageSize);
        setRestaurants(data.items);
        setTotalCount(data.totalCount);
        setTotalPages(data.totalPages);
      } catch (err) {
        setError("Failed to load restaurants.");
      } finally {
        setLoading(false);
      }
    };

    fetchRestaurants();
  }, [pageNumber, pageSize]);

  const handleNextPage = () => setPageNumber((prev) => prev + 1);
  const handlePreviousPage = () => setPageNumber((prev) => Math.max(prev - 1, 1));
  const handlePageSizeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setPageSize(Number(event.target.value));
    setPageNumber(1);
  };

  const openModal = (restaurantId: string) => {
    setSelectedRestaurantId(restaurantId);
  };

  const closeModal = () => {
    setSelectedRestaurantId(null);
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div>
      <h1>Restaurants List</h1>

      <div>
        <label htmlFor="pageSize">Page Size: </label>
        <select id="pageSize" value={pageSize} onChange={handlePageSizeChange}>
          <option value={5}>5</option>
          <option value={10}>10</option>
          <option value={20}>20</option>
        </select>
      </div>

      <p>
        Showing {restaurants.length} of {totalCount} restaurants
      </p>

      <ul>
        {restaurants.map((restaurant) => (
          <li key={restaurant.id}>
            <h2>{restaurant.name}</h2>
            <p>
              {restaurant.address.street}, {restaurant.address.city},{" "}
              {restaurant.address.country}
            </p>
            <p>
              Opening Hours: {restaurant.openingHour} - {restaurant.closingHour}
            </p>
            <button
              className="btn-book"
              onClick={() => openModal(restaurant.id)}
            >
              Book a Table
            </button>
          </li>
        ))}
      </ul>

      <div>
        <button onClick={handlePreviousPage} disabled={pageNumber === 1}>
          Previous Page
        </button>
        <span>
          Page {pageNumber} of {totalPages}
        </span>
        <button onClick={handleNextPage} disabled={pageNumber >= totalPages}>
          Next Page
        </button>
      </div>

      <Modal
        isOpen={!!selectedRestaurantId}
        onClose={closeModal}
        title="Book a Table"
      >
        {selectedRestaurantId && (
          <BookTableForm
            restaurantId={selectedRestaurantId}
            onClose={closeModal}
          />
        )}
      </Modal>
    </div>
  );
};

export default HomePage;
