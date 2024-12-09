import { ReservationStatus } from "../components/enums/ReservationStatus";

export interface UserReservationDto {
    id: string;
    reservationDate: string; // ISO date string
    startTime: string; // ISO time string
    endTime: string; // ISO time string
    numberOfSeats: number;
    status: ReservationStatus; 
    restaurantName: string;
    restaurantId: string;
  }