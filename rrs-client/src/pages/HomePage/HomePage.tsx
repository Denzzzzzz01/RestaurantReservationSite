import React, { useEffect, useState } from "react";
import { RestaurantDto } from "../../models/RestaurantDTO";
import { getRestaurants } from "../../services/RestaurantService";

type Props = {};

const HomePage = (props: Props) => {
  const [restaurants, setRestaurants] = useState<RestaurantDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);
  const [hasMore, setHasMore] = useState<boolean>(true); 

  useEffect(() => {
    const fetchRestaurants = async () => {
      setLoading(true);  
      setError(null);    

      try {
        const data = await getRestaurants(pageNumber, pageSize);

        if (data.length === 0) {
          setHasMore(false);
        } else {
          setRestaurants(data);
          setHasMore(true);
        }
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

      <ul>
        {restaurants.map((restaurant) => (
          <li key={restaurant.id}>
            <h2>{restaurant.name}</h2>
            <p>{restaurant.address.street}, {restaurant.address.city}, {restaurant.address.country}</p>
            <p>Seating Capacity: {restaurant.seatingCapacity}</p>
            <p>Opening Hours: {restaurant.openingHour} - {restaurant.closingHour}</p>
          </li>
        ))}
      </ul>

      <div>
        <button onClick={handlePreviousPage} disabled={pageNumber === 1}>
          Previous Page
        </button>
        <span> Page {pageNumber} </span>
        <button onClick={handleNextPage} disabled={!hasMore}>
          Next Page
        </button>
      </div>
    </div>
  );
};

export default HomePage;
