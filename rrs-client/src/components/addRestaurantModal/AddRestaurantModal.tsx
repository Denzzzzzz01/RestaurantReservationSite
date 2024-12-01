import React, { useState } from "react";
import { createRestaurant } from "../../services/RestaurantService";
import Modal from "../modal/Modal";
import { CreateRestaurantDto } from "../../models/CreateRestaurantDto";
import { toastPromise, notifyError } from "../../utils/toastUtils";
import { useAuth } from "../../context/useAuth";

interface AddRestaurantModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const AddRestaurantModal: React.FC<AddRestaurantModalProps> = ({ isOpen, onClose }) => {
  const { refreshToken } = useAuth(); 
  const [restaurant, setRestaurant] = useState<Omit<CreateRestaurantDto, "id">>({
    name: "",
    address: {
      street: "",
      city: "",
      country: "",
      state: "",
      latitude: 0,
      longitude: 0,
    },
    openingHour: "09:00:00",
    closingHour: "21:00:00",
    phoneNumber: "",
    website: "",
  });
  const [loading, setLoading] = useState<boolean>(false);

  const handleChange = (field: string, value: any) => {
    setRestaurant((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleAddressChange = (field: string, value: any) => {
    setRestaurant((prev) => ({
      ...prev,
      address: {
        ...prev.address,
        [field]: value,
      },
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      await toastPromise(
        createRestaurant(restaurant),
        "Adding the restaurant...",
        "Restaurant added successfully!",
        "Failed to add the restaurant"
      );

      await refreshToken(); 
      
      onClose(); 
    } catch (error) {
      console.error("Error adding restaurant:", error);
      notifyError("An unexpected error occurred. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal title="Add Restaurant" isOpen={isOpen} onClose={onClose}>
      <form onSubmit={handleSubmit}>
        <div className="mb-2">
          <label className="block">Name</label>
          <input
            type="text"
            value={restaurant.name}
            onChange={(e) => handleChange("name", e.target.value)}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">Street</label>
          <input
            type="text"
            value={restaurant.address.street}
            onChange={(e) => handleAddressChange("street", e.target.value)}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">City</label>
          <input
            type="text"
            value={restaurant.address.city}
            onChange={(e) => handleAddressChange("city", e.target.value)}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">Country</label>
          <input
            type="text"
            value={restaurant.address.country}
            onChange={(e) => handleAddressChange("country", e.target.value)}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">State</label>
          <input
            type="text"
            value={restaurant.address.state}
            onChange={(e) => handleAddressChange("state", e.target.value)}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">Latitude</label>
          <input
            type="number"
            value={restaurant.address.latitude}
            onChange={(e) => handleAddressChange("latitude", parseFloat(e.target.value))}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">Longitude</label>
          <input
            type="number"
            value={restaurant.address.longitude}
            onChange={(e) => handleAddressChange("longitude", parseFloat(e.target.value))}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">Opening Hour</label>
          <input
            type="time"
            value={restaurant.openingHour}
            onChange={(e) => handleChange("openingHour", e.target.value)}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">Closing Hour</label>
          <input
            type="time"
            value={restaurant.closingHour}
            onChange={(e) => handleChange("closingHour", e.target.value)}
            className="border p-2 w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block">Phone Number</label>
          <input
            type="text"
            value={restaurant.phoneNumber}
            onChange={(e) => handleChange("phoneNumber", e.target.value)}
            className="border p-2 w-full"
          />
        </div>
        <div className="mb-2">
          <label className="block">Website</label>
          <input
            type="text"
            value={restaurant.website}
            onChange={(e) => handleChange("website", e.target.value)}
            className="border p-2 w-full"
          />
        </div>
        <div className="mt-4">
          <button
            type="submit"
            className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
            disabled={loading}
          >
            {loading ? "Adding..." : "Add Restaurant"}
          </button>
        </div>
      </form>
    </Modal>
  );
};

export default AddRestaurantModal;
