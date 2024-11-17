import React, { useState } from "react";
import { BookTableDto } from "../../models/BookTableDto";
import { bookTable } from "../../services/ReservationService";

type Props = {
  restaurantId: string;
};

const BookTableForm: React.FC<Props> = ({ restaurantId }) => {
  const [reservationDate, setReservationDate] = useState<string>("");
  const [startTime, setStartTime] = useState<string>("12:00");
  const [numberOfSeats, setNumberOfSeats] = useState<number>(1);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(null);
  
    try {
      const isoDateTime = new Date(reservationDate).toISOString();
      const formattedStartTime = `${startTime}:00`;
  
      const dto: BookTableDto = {
        restaurantId,
        reservationDate: isoDateTime,
        startTime: formattedStartTime,
        numberOfSeats,
      };
  
      console.log("Prepared DTO:", dto); 
  
      await bookTable(dto);
      setSuccess("Table booked successfully!");
    } catch (err: unknown) {
      if (err instanceof Error) {
        console.error("Booking error:", err); 
        setError(err.message);
      } else {
        console.error("Unexpected booking error:", err); 
        setError("An unexpected error occurred.");
      }
    }
  };
  

  return (
    <form onSubmit={handleSubmit} style={{ border: "1px solid #ccc", padding: "16px", borderRadius: "8px" }}>
      <h2>Book a Table !!!</h2>
      
      {error && <p style={{ color: "red" }}>{error}</p>}
      {success && <p style={{ color: "green" }}>{success}</p>}

      <div>
        <label htmlFor="reservationDate">Date: </label>
        <input
          id="reservationDate"
          type="date"
          value={reservationDate}
          onChange={(e) => setReservationDate(e.target.value)}
          required
        />
      </div>

      <div>
        <label htmlFor="startTime">Start Time: </label>
        <input
          id="startTime"
          type="time"
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
          required
        />
      </div>

      <div>
        <label htmlFor="numberOfSeats">Number of Seats: </label>
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

      <button type="submit" style={{ marginTop: "12px" }}>
        Book Table
      </button>
    </form>
  );
};

export default BookTableForm;
