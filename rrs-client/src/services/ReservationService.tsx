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

export const getRestaurantReservations = async (
  restaurantId: string,
  filter: number
): Promise<UserReservationDto[]> => {
  try {
    const response = await axios.get<UserReservationDto[]>(
      `${API_URL}/restaurant/${restaurantId}`,
      {
        params: { filter },
      }
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching restaurant reservations:", error);
    throw error;
  }
};

export const cancelReservation = async (reservationId: string): Promise<void> => {
  try {
    await axios.post(`${API_URL}/cancel`, null, {
      params: { reservationId },
    });
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error(
        "Error response from server:",
        error.response?.data || "No response data"
      );
      throw new Error(
        error.response?.data?.message || "Failed to cancel reservation. Please try again."
      );
    } else {
      console.error("Unexpected error:", error);      
      throw new Error("An unexpected error occurred.");
    }
  }
};



