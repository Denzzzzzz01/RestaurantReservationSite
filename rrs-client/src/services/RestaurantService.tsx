import axios from "axios";
import { RestaurantDto } from "../models/RestaurantDto";
import { PagedResult } from "../models/PagedResult";
import { CreateRestaurantDto } from "../models/CreateRestaurantDto";

const API_URL = "/api/Restaurants";

export const getRestaurants = async (
  pageNumber: number,
  pageSize: number
) : Promise<PagedResult<RestaurantDto>> => {
  try {
  const response = await axios.get(API_URL, {
    params: { PageNumber: pageNumber, PageSize: pageSize },
  });

  return response.data;

  } catch (error) {
    console.error("Error fetching restaurants", error);
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