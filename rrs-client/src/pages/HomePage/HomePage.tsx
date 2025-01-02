import React, { useCallback, useEffect, useState } from "react";
import { RestaurantDto } from "../../models/RestaurantDto";
import { getRestaurants, searchRestaurants } from "../../services/RestaurantService";
import BookTableForm from "../../components/bookTableForm/BookTableForm";
import Modal from "../../components/modal/Modal";
import { debounce } from "lodash";
import PaginationControls from "../../components/paginationControls/PaginationControls";
import RestaurantsList from "../../components/restaurantsList/RestaurantsList";
import SearchBar from "../../components/searchBar/SearchBar";
import './HomePage.scss';

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
      if (query.length === 0) {
        setIsSearching(false);
        return;
      } else if (query.length < 3) {
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
    <div className="homepage">
      <div className="homepage-container">
        <div className="header-section">
          <div className="header-controls">
          <SearchBar
            value={searchQuery}
            onChange={handleSearchChange}
            placeholder="Enter restaurant name or address"
            height="3rem"
            fontSize="1.5rem"
          />
          <div className="page-size-container">
            <label htmlFor="pageSize">Show: </label>
            <select id="pageSize" value={pageSize} onChange={handlePageSizeChange}>
              <option value={2}>2</option>
              <option value={10}>10</option>
              <option value={20}>20</option>
            </select>
          </div>
        </div>
      </div>
        
        {loading ? (
          <p>Loading...</p>
        ) : error ? (
          <p style={{ color: "red" }}>{error}</p>
        ) : (
          <RestaurantsList restaurants={restaurants} onBookTable={openModal} />
        )}

        {!isSearching && (
          <div className="pagination-controls">
          <p className="restaurants-info">
            Showing {restaurants.length} of {totalCount} restaurants
          </p>

            <PaginationControls
              pageNumber={pageNumber}
              totalPages={totalPages}
              onNext={handleNextPage}
              onPrevious={handlePreviousPage}
            />
          </div>
        )}

        <div className="modal-container">
          <Modal isOpen={!!selectedRestaurantId} onClose={closeModal} title="Book a Table">
            {selectedRestaurantId && (
              <BookTableForm restaurantId={selectedRestaurantId} onClose={closeModal} />
            )}
          </Modal>
        </div>
      </div>
    </div>
  );
  
};

export default HomePage;
