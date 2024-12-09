import axios from "axios";
import { TableDto } from "../models/TableDto";
import { CreateTablesDto } from "../models/CreateTablesDto";

const API_URL = "/api/Tables";

export const getRestaurantTables = async (restaurantId: string): Promise<TableDto[]> => {
  try {
    const response = await axios.get<TableDto[]>(`${API_URL}/restaurants/${restaurantId}/tables`);
    return response.data;
  } catch (error) {
    console.error("Error fetching tables:", error);
    throw new Error("Failed to fetch tables. Please try again later.");
  }
};

export const createTables = async (
    restaurantId: string,
    dto: CreateTablesDto
  ): Promise<void> => {
    try {
      await axios.post(`${API_URL}/${restaurantId}/tables`, dto, {
        headers: { "Content-Type": "application/json" },
      });
    } catch (error) {
      console.error("Error creating tables:", error);
      throw new Error("Failed to create tables. Please try again.");
    }
  };