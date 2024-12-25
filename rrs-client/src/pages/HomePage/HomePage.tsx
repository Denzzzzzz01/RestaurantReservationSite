import React, { useCallback, useEffect, useState } from "react";
import { RestaurantDto } from "../../models/RestaurantDto";
import { getRestaurants, searchRestaurants } from "../../services/RestaurantService";
import BookTableForm from "../../components/bookTableForm/BookTableForm";
import Modal from "../../components/modal/Modal";
import { debounce } from "lodash";

const HomePage: React.FC = () => {
  const [restaurants, setRestaurants] = useState<RestaurantDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);
  const [totalCount, setTotalCount] = useState<number>(0);
  const [totalPages, setTotalPages] = useState<number>(1);

  const [searchQuery, setSearchQuery] = useState<string>("");
  const [isSearching, setIsSearching] = useState<boolean>(false);

  const [selectedRestaurantId, setSelectedRestaurantId] = useState<string | null>(null);

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

  useEffect(() => {
    if (!isSearching) {
      fetchRestaurants();
    }
  }, [pageNumber, pageSize, isSearching]);

  const debouncedSearch = useCallback(
    debounce(async (query: string) => {
      if(query.length === 0)
      {
        setIsSearching(false);
        return;
      }
      else if (query.length < 3) {
        return;
      }

      setIsSearching(true);
      setLoading(true);

      try {
        const data = await searchRestaurants(query, 1, pageSize);
        setRestaurants(data.items);
        setTotalCount(data.totalCount);
        setTotalPages(data.totalPages);
        setPageNumber(1);

        console.log(data.items);
      } catch (err) {
        setError("Failed to search restaurants.");
      } finally {
        setLoading(false);
      }
    }, 500),
    [pageSize]
  );

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const query = event.target.value;
    setSearchQuery(query);
    debouncedSearch(query);
  };

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

  return (
    <div>
      <h1><i className="fi fi-ss-user"></i>Restaurants List</h1>
      

      <div>
        <label htmlFor="search">Search Restaurants: </label>
        <input
          id="search"
          type="text"
          value={searchQuery}
          onChange={handleSearchChange}
          placeholder="Enter restaurant name"
        />
      </div>

      <div>
        <label htmlFor="pageSize">Page Size: </label>
        <select id="pageSize" value={pageSize} onChange={handlePageSizeChange}>
          <option value={2}>2</option>
          <option value={10}>10</option>
          <option value={20}>20</option>
        </select>
      </div>

      <p>
        Showing {restaurants.length} of {totalCount} restaurants
      </p>

      <div>
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p style={{ color: "red" }}>{error}</p>
      ) : restaurants.length === 0 ? (
        <p>No restaurants found. Try a different search.</p>
      ) : (
        <ul>
          {restaurants.map((restaurant) => (
            <li key={restaurant.id}>
              <div style={{ display: "flex", alignItems: "center", marginBottom: "1rem" }}>
                <img
                  src={`${restaurant.logoUrl}`}
                  alt={`${restaurant.name} Logo`}
                  style={{
                    width: "80px",
                    height: "80px",
                    borderRadius: "8px",
                    objectFit: "cover",
                    marginRight: "1rem",
                  }}
                />
                <div>
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
                </div>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>

      {!isSearching && (
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
      )}

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
