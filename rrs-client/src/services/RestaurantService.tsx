import axios from "axios";
import { RestaurantDto } from "../models/RestaurantDTO";

const API_URL = "/api/Restaurants";

export const getRestaurants = async (pageNumber: number = 1, pageSize: number = 10): Promise<RestaurantDto[]> => {
  try {
    const response = await axios.get<RestaurantDto[]>(API_URL, {
      params: {
        PageNumber: pageNumber,
        PageSize: pageSize,
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching restaurants", error);
    throw error;
  }
};