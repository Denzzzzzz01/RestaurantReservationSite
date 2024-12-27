import { Address } from "./Address";

export interface RestaurantUpdateDto {
    name: string;
    description: string;
    address: Address;
    openingHour: string;
    closingHour: string;
    phoneNumber: string;
    website: string | null;
  }