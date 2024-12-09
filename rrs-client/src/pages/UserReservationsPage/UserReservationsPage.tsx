import React, { useEffect, useState } from "react";
import { getUserReservations, cancelReservation } from "../../services/ReservationService";
import { UserReservationDto } from "../../models/ReservationUserDto";
import { notifyError, notifySuccess, notifyWarning } from "../../utils/toastUtils";
import { getStatusLabel, ReservationStatus } from "../../components/enums/ReservationStatus";

const UserReservationsPage: React.FC = () => {
  const [reservations, setReservations] = useState<UserReservationDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedReservation, setSelectedReservation] = useState<UserReservationDto | null>(null);

  const fetchReservations = async () => {
    try {
      setLoading(true);
      const data = await getUserReservations();
  
      const transformedData = data.map((reservation) => ({
        ...reservation,
        status: reservation.status as ReservationStatus,
      }));
  
      setReservations(transformedData);
    } catch (error) {
      console.error("Error fetching reservations:", error);
      notifyError("Failed to load reservations. Please try again later.");
    } finally {
      setLoading(false);
    }
  };
  
  
  useEffect(() => {
    fetchReservations();
  }, []);
  
  const handleCancelReservation = async () => {
    if (!selectedReservation) return;
  
    if (selectedReservation.status === ReservationStatus.Cancelled) {
      notifyWarning("This reservation is already cancelled.");
      return;
    }
  
    try {
      await cancelReservation(selectedReservation.id);
      notifySuccess(
        `Reservation for ${selectedReservation.restaurantName} canceled successfully.`
      );
      setSelectedReservation(null);
      fetchReservations(); 
    } catch (error) {
      notifyError("Failed to cancel reservation. Please try again later.");
      console.error("Error canceling reservation:", error);
    }
  };
  
  

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
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {reservations.map((reservation) => (
            <tr key={reservation.id}>
              <td>{new Date(reservation.reservationDate).toLocaleDateString()}</td>
              <td>{reservation.startTime}</td>
              <td>{reservation.endTime}</td>
              <td>{reservation.numberOfSeats}</td>
              <td>{getStatusLabel(reservation.status)}</td>
              <td>
                <a href={`/restaurants/${reservation.restaurantId}`}>{reservation.restaurantName}</a>
              </td>
              {reservation.status === 0 && (
                <td>
                  <button
                    onClick={() => setSelectedReservation(reservation)}
                    className="cancel-reservation-btn"
                  >
                    Cancel
                  </button>
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>


      {selectedReservation && (
        <div className="confirmation-modal">
          <div className="confirmation-modal-content">
            <h2>Cancel Reservation</h2>
            <p>
              Are you sure you want to cancel your reservation for{" "}
              <strong>{selectedReservation.restaurantName}</strong> on{" "}
              {new Date(selectedReservation.reservationDate).toLocaleDateString()} at{" "}
              {selectedReservation.startTime}?
            </p>
            <div className="confirmation-modal-actions">
              <button onClick={() => setSelectedReservation(null)}>No, Keep It</button>
              <button onClick={handleCancelReservation}>Yes, Cancel</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default UserReservationsPage;
