import React, { useEffect, useState } from "react";
import { getRestaurantReservations } from "../../services/ReservationService";
import { UserReservationDto } from "../../models/ReservationUserDto";
import { useParams } from "react-router-dom";
import { notifyError } from "../../utils/toastUtils";
import { getStatusLabel } from "../../components/enums/ReservationStatus";

const RestaurantReservationsPage: React.FC = () => {
  const { restaurantId } = useParams<{ restaurantId: string }>(); 
  const [reservations, setReservations] = useState<UserReservationDto[]>([]);
  const [filter, setFilter] = useState<number>(0);
  const [loading, setLoading] = useState(false);

  const fetchReservations = async () => {
    if (!restaurantId) {
      notifyError("Restaurant ID is missing!");
      return;
    }

    try {
      setLoading(true);
      const data = await getRestaurantReservations(restaurantId, filter);
      setReservations(data);
    } catch (error) {
      notifyError("Failed to fetch reservations. Please try again later.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchReservations();
  }, [filter, restaurantId]);

  return (
    <div>
      <h2>Reservations for Restaurant</h2>

      <div>
        <label>Filter:</label>
        <select value={filter} onChange={(e) => setFilter(Number(e.target.value))}>
          <option value={0}>All</option>
          <option value={1}>Active</option>
        </select>
      </div>

      {loading ? (
        <p>Loading reservations...</p>
      ) : reservations.length > 0 ? (
        <ul>
          {reservations.map((reservation) => (
            <li key={reservation.id}>
              <strong>{reservation.restaurantName}</strong>
              <p>
                {new Date(reservation.reservationDate).toLocaleDateString()} |{" "}
                {reservation.startTime} - {reservation.endTime}
              </p>
              <p>Seats: {reservation.numberOfSeats}</p>
              <p>Status: {getStatusLabel(reservation.status)}</p>
            </li>
          ))}
        </ul>
      ) : (
        <p>No reservations found.</p>
      )}
    </div>
  );
};

export default RestaurantReservationsPage;
