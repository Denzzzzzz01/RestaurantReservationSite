import { Address } from "./Address";
import { RestaurantManagerDto } from "./RestaurantManagerDto";

export interface DetailedRestaurantDto {
    id: string;
    name: string;
    description: string;
    address: Address;
    logoUrl: string;
    openingHour: string;
    closingHour: string;
    phoneNumber: string;
    website: string | null;
    manageres: RestaurantManagerDto[];
  }

  