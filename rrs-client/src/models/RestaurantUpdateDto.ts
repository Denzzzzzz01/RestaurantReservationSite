import { Address } from "./Address";

export interface RestaurantUpdateDto {
    name: string;
    address: Address;
    openingHour: string;
    closingHour: string;
    phoneNumber: string;
    website: string | null;
  }