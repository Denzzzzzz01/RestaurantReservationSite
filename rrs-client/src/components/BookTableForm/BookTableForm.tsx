import React, { useState } from "react";
import { BookTableDto } from "../../models/BookTableDto";
import { bookTable } from "../../services/ReservationService";
import { toastPromise, notifyError } from "../../utils/toastUtils";

type Props = {
  restaurantId: string;
  onClose: () => void;
};

const BookTableForm: React.FC<Props> = ({ restaurantId, onClose }) => {
  const [reservationDate, setReservationDate] = useState<string>("");
  const [startTime, setStartTime] = useState<string>("12:00");
  const [numberOfSeats, setNumberOfSeats] = useState<number>(1);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const isoDateTime = new Date(reservationDate).toISOString();
    const formattedStartTime = `${startTime}:00`;

    const dto: BookTableDto = {
      restaurantId,
      reservationDate: isoDateTime,
      startTime: formattedStartTime,
      numberOfSeats,
    };

    try {
      await toastPromise(
        bookTable(dto),
        "Processing your reservation...",
        "Table booked successfully!",
        "Failed to book the table"
      );
      onClose(); // Close the modal on success
    } catch (error) {
      console.error("Booking error:", error);
      notifyError("An unexpected error occurred while booking.");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="book-table-form">
      <h2>Book a Table</h2>
      <div className="form-group">
        <label htmlFor="reservationDate">Date:</label>
        <input
          id="reservationDate"
          type="date"
          value={reservationDate}
          onChange={(e) => setReservationDate(e.target.value)}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="startTime">Start Time:</label>
        <input
          id="startTime"
          type="time"
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="numberOfSeats">Number of Seats:</label>
        <input
          id="numberOfSeats"
          type="number"
          value={numberOfSeats}
          onChange={(e) => setNumberOfSeats(Number(e.target.value))}
          min="1"
          max="500"
          required
        />
      </div>

      <button type="submit" className="btn-submit">
        Book Table
      </button>
    </form>
  );
};

export default BookTableForm;
