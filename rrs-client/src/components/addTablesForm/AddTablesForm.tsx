import React, { useState } from "react";

interface AddTablesFormProps {
  onSubmit: (numberOfTables: number, tableCapacity: number, description: string) => void;
}

const AddTablesForm: React.FC<AddTablesFormProps> = ({ onSubmit }) => {
  const [numberOfTables, setNumberOfTables] = useState(1);
  const [tableCapacity, setTableCapacity] = useState(1);
  const [description, setDescription] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(numberOfTables, tableCapacity, description);
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Number of Tables:</label>
        <input
          type="number"
          min="1"
          value={numberOfTables}
          onChange={(e) => setNumberOfTables(Number(e.target.value))}
          required
        />
      </div>
      <div>
        <label>Table Capacity:</label>
        <input
          type="number"
          min="1"
          value={tableCapacity}
          onChange={(e) => setTableCapacity(Number(e.target.value))}
          required
        />
      </div>
      <div>
        <label>Description:</label>
        <textarea
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        ></textarea>
      </div>
      <button type="submit">Add Tables</button>
    </form>
  );
};

export default AddTablesForm;
