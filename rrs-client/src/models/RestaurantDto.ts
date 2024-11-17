import { Address } from "./Address";

export interface RestaurantDto {
    id: string;
    name: string;
    address: Address;
    openingHour: string;
    closingHour: string;
  }