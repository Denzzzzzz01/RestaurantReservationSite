import { Address } from "./Address";

export interface RestaurantDto {
    id: string;
    name: string;
    address: Address;
    logoUrl: string;
    openingHour: string;
    closingHour: string;
  }