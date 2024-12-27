import React from "react";
import { TableDto } from "../../models/TableDto";

interface RestaurantTablesProps {
  tables: TableDto[];
}

const RestaurantTables: React.FC<RestaurantTablesProps> = ({ tables }) => {
  return (
    <div>
      <h3>Tables:</h3>
      {tables.length > 0 ? (
        <ul>
          {tables.map((table) => (
            <li key={table.id}>
              <p>Table Number: {table.tableNumber}</p>
              <p>Capacity: {table.capacity}</p>
              <p>Description: {table.description}</p>
              <p>Status: {table.isAvailable ? "Available" : "Unavailable"}</p>
            </li>
          ))}
        </ul>
      ) : (
        <p>No tables available.</p>
      )}
    </div>
  );
};

export default RestaurantTables;
