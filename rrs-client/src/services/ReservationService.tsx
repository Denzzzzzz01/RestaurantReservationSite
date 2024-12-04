import axios from "axios";
import { BookTableDto } from "../models/BookTableDto";
import { UserReservationDto } from "../models/ReservationUserDto";

const API_URL = "/api/Reservations";

export const bookTable = async (dto: BookTableDto): Promise<void> => {
    try {
      console.log("Sending DTO:", dto); 
      await axios.post(`${API_URL}/book-table`, dto, {
        headers: {
          "Content-Type": "application/json", 
        },
      });
    
  } catch (error: unknown) {
    if (axios.isAxiosError(error)) {
      throw new Error(
        error.response?.data?.message || "Failed to book table. Please try again."
      );
    } else {
      throw new Error("An unexpected error occurred.");
    }
  }
};

export const getUserReservations = async (): Promise<UserReservationDto[]> => {
  try {
    const response = await axios.get<UserReservationDto[]>(`${API_URL}/user`);
    return response.data;
  } catch (error) {
    console.error("Error fetching user reservations:", error);
    throw error;
  }
};
