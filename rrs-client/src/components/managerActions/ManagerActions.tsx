import React from "react";

interface ManagerActionsProps {
  onAddTables: () => void;
  onUpdateRestaurant: () => void;
  onChangeLogo: () => void;
}

const ManagerActions: React.FC<ManagerActionsProps> = ({
  onAddTables,
  onUpdateRestaurant,
  onChangeLogo,
}) => {
  return (
    <div>
      <button onClick={onAddTables}>Add Tables</button>
      <button onClick={onUpdateRestaurant}>Update Restaurant</button>
      <button onClick={onChangeLogo}>Change Logo</button>
    </div>
  );
};

export default ManagerActions;
