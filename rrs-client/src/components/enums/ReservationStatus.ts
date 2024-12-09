export enum ReservationStatus {
    Active,
    Completed,
    Cancelled,
}

export const getStatusLabel = (status: ReservationStatus): string => {
  switch (status) {
    case ReservationStatus.Active:
      return "Active";
    case ReservationStatus.Completed:
      return "Completed";
    case ReservationStatus.Cancelled:
      return "Cancelled";
    default:
      return "Unknown";
  }
};