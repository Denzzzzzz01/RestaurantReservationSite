export interface CreateRestaurantDto {
    name: string;
    address: {
      street: string;
      city: string;
      country: string;
      state: string;
      latitude: number;
      longitude: number;
    };
    openingHour: string;
    closingHour: string;
    phoneNumber: string;
    website: string;
  }