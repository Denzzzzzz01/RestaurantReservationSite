import { Address } from "./Address";

export interface RestaurantDto {
    id: string;
    name: string;
    address: Address;
    seatingCapacity: number;
    openingHour: string;
    closingHour: string;
  }