import { Address } from "./Address";
import { RestaurantManagerDto } from "./RestaurantManagerDto";

export interface DetailedRestaurantDto {
    id: string;
    name: string;
    address: Address;
    openingHour: string;
    closingHour: string;
    phoneNumber: string;
    website: string | null;
    manageres: RestaurantManagerDto[];
  }

  