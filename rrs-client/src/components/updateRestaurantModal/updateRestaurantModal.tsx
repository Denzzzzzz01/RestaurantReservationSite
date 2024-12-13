import React, { useState } from "react";
import { DetailedRestaurantDto } from "../../models/DetailedRestaurantDto";
import { RestaurantUpdateDto } from "../../models/RestaurantUpdateDto";

interface UpdateRestaurantFormProps {
  initialData: DetailedRestaurantDto;
  onSubmit: (updatedData: RestaurantUpdateDto) => Promise<void>;
  onClose: () => void;
}

const UpdateRestaurantForm: React.FC<UpdateRestaurantFormProps> = ({
  initialData,
  onSubmit,
  onClose,
}) => {
  const [formData, setFormData] = useState<RestaurantUpdateDto>(initialData);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await onSubmit(formData);
      onClose();
    } catch (error) {
      console.error("Failed to update restaurant:", error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label htmlFor="name">Name:</label>
        <input
          id="name"
          name="name"
          value={formData.name}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label htmlFor="street">Street:</label>
        <input
          id="street"
          name="address.street"
          value={formData.address.street}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label htmlFor="city">City:</label>
        <input
          id="city"
          name="address.city"
          value={formData.address.city}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label htmlFor="country">Country:</label>
        <input
          id="country"
          name="address.country"
          value={formData.address.country}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label htmlFor="openingHour">Opening Hour:</label>
        <input
          id="openingHour"
          name="openingHour"
          value={formData.openingHour}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label htmlFor="closingHour">Closing Hour:</label>
        <input
          id="closingHour"
          name="closingHour"
          value={formData.closingHour}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label htmlFor="phoneNumber">Phone Number:</label>
        <input
          id="phoneNumber"
          name="phoneNumber"
          value={formData.phoneNumber}
          onChange={handleChange}
          required
        />
      </div>
      <button type="submit">Update Restaurant</button>
      <button type="button" onClick={onClose}>
        Cancel
      </button>
    </form>
  );
};

export default UpdateRestaurantForm;
