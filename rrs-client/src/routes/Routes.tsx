import { createBrowserRouter } from "react-router-dom";
import App from "../components/app/App";
import HomePage from "../pages/HomePage/HomePage";
import LoginPage from "../pages/LoginPage/LoginPage";
import RegisterPage from "../pages/RegisterPage/RegisterPage";
import ProtectedRoute from "./ProtectedRoute";
import RestaurantPage from "../pages/RestaurantPage/RestaurantPage";
import UserReservationsPage from "../pages/UserReservationsPage/UserReservationsPage";


export const router = createBrowserRouter([
    {
      path: "/",
      element: <App />,
      children: [
        { path: "", element: <HomePage /> },
        { path: "restaurants/:id", element: <RestaurantPage /> }, 
        { path: "reservations", element: <ProtectedRoute><UserReservationsPage /></ProtectedRoute> },
        { path: "login", element: <LoginPage /> },
        { path: "register", element: <RegisterPage /> },
      ],
    },
  ]);