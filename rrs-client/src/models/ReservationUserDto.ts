export interface UserReservationDto {
    id: string;
    reservationDate: string; // ISO date string
    startTime: string; // ISO time string
    endTime: string; // ISO time string
    numberOfSeats: number;
    status: string; 
    restaurantName: string;
    restaurantId: string;
  }