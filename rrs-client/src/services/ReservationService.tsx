import axios from "axios";
import { BookTableDto } from "../models/BookTableDto";

const API_URL = "/api/reservations/book-table";

export const bookTable = async (dto: BookTableDto): Promise<void> => {
    try {
      console.log("Sending DTO:", dto); 
      await axios.post(API_URL, dto, {
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
