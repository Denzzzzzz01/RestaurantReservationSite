import axios from "axios";
import { RestaurantDto } from "../models/RestaurantDto";
import { PagedResult } from "../models/PagedResult";
import { CreateRestaurantDto } from "../models/CreateRestaurantDto";
import { DetailedRestaurantDto } from "../models/DetailedRestaurantDto";
import { RestaurantUpdateDto } from "../models/RestaurantUpdateDto";

const API_URL = "/api/Restaurants";

export const getRestaurants = async (
  pageNumber: number,
  pageSize: number
) : Promise<PagedResult<RestaurantDto>> => {
  try {
  const response = await axios.get(API_URL, {
    params: { PageNumber: pageNumber, PageSize: pageSize },
  });

  console.log(response.data);
  return response.data;

  } catch (error) {
    console.error("Error fetching restaurants", error);
    throw error;
  }
};

export const getRestaurantById = async (id: string): Promise<DetailedRestaurantDto> => {
  try {
    const response = await axios.get(`/api/Restaurants/${id}`);

    console.log(response.data);
    return response.data;
  } catch (error) {
    console.error("Error fetching restaurant by ID", error);
    throw error;
  }
};


export const createRestaurant = async (
  restaurant: CreateRestaurantDto
): Promise<void> => {
  try {
    await axios.post(API_URL, restaurant);
  } catch (error) {
    console.error("Error creating restaurant", error);
    throw error;
  }
};

export const updateRestaurant = async (
  id: string,
  updatedRestaurant: RestaurantUpdateDto
): Promise<void> => {
  const response = await axios.put(`/api/Restaurants/${id}`, updatedRestaurant);
  return response.data;
};

export const getUserRestaurant = async (): Promise<{ id: string; name: string }> => {
  try {
    const response = await axios.get(`/api/account/restaurant`);
    return response.data;
  } catch (error) {
    console.error("Error fetching user's restaurant", error);
    throw error;
  }
};

export const searchRestaurants = async (
  query: string,
  pageNumber: number,
  pageSize: number
): Promise<PagedResult<RestaurantDto>> => {
  try {
    const response = await axios.get(`${API_URL}/search`, {
      params: { query, pageNumber, pageSize },
    });
    
    return response.data;
  } catch (error) {
    console.error("Error searching restaurants:", error);
    throw new Error("Failed to search restaurants. Please try again.");
  }
};

export const uploadRestaurantLogo = async (restaurantId: string, formData: FormData): Promise<void> => {
  try {
    await axios.post(`/api/Restaurants/upload-logo/${restaurantId}`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  } catch (error) {
    console.error("Error uploading logo:", error);
    throw error;
  }
};
