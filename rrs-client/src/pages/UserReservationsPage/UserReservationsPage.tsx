import React, { useEffect, useState } from "react";
import { UserReservationDto } from "../../models/ReservationUserDto";
import { getUserReservations } from "../../services/ReservationService";

const UserReservationsPage: React.FC = () => {
  const [reservations, setReservations] = useState<UserReservationDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchReservations = async () => {
      try {
        setLoading(true);
        const data = await getUserReservations();
        setReservations(data);
      } catch (error) {
        console.error("Error fetching reservations:", error);
        setError("Failed to load reservations. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchReservations();
  }, []);

  if (loading) {
    return <div>Loading reservations...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  if (reservations.length === 0) {
    return <div>No reservations found.</div>;
  }

  return (
    <div className="reservations-page">
      <h1>My Reservations</h1>
      <table className="reservations-table">
        <thead>
          <tr>
            <th>Date</th>
            <th>Start Time</th>
            <th>End Time</th>
            <th>Seats</th>
            <th>Status</th>
            <th>Restaurant</th>
          </tr>
        </thead>
        <tbody>
          {reservations.map((reservation) => (
            <tr key={reservation.id}>
              <td>{new Date(reservation.reservationDate).toLocaleDateString()}</td>
              <td>{reservation.startTime}</td>
              <td>{reservation.endTime}</td>
              <td>{reservation.numberOfSeats}</td>
              <td>{reservation.status}</td>
              <td>
                <a href={`/restaurants/${reservation.restaurantId}`}>{reservation.restaurantName}</a>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default UserReservationsPage;
