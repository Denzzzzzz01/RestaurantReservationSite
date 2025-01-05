import { createContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import React from "react";
import axios from "axios";
import { UserProfile } from "../models/User";
import { loginAPI, registerAPI } from "../services/AuthService";
import { notifySuccess, notifyWarning } from "../utils/toastUtils";
import { jwtDecode } from "jwt-decode";

type UserContextType = {
  user: UserProfile | null;
  token: string | null;
  registerUser: (username: string, email: string, password: string, passwordConfirm: string) => void;
  loginUser: (email: string, password: string) => void;
  logout: () => void;
  isLoggedIn: () => boolean;
  refreshToken: () => Promise<void>; 
};

type Props = { children: React.ReactNode };

const UserContext = createContext<UserContextType>({} as UserContextType);

export const UserProvider = ({ children }: Props) => {
  const navigate = useNavigate();
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<UserProfile | null>(null);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const user = localStorage.getItem("user");
    const token = localStorage.getItem("token");
    if (user && token) {
      setUser(JSON.parse(user));
      setToken(token);
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    }
    setIsReady(true);
  }, []);

  const loginUser = async (email: string, password: string) => {
    await loginAPI(email, password)
      .then((res) => {
        if (res) {
          localStorage.setItem("token", res?.data.token);
          const userObj = {
            username: res?.data.username,
            email: res?.data.email,
          };
          localStorage.setItem("user", JSON.stringify(userObj));
          setToken(res?.data.token!);
          setUser(userObj!);
          axios.defaults.headers.common["Authorization"] = "Bearer " + res?.data.token;
          notifySuccess("Login Success!");
          navigate("/");
        }
      })
      .catch(() => notifyWarning("Server error occurred"));
  };

  const registerUser = async (username: string, email: string, password: string, confirmPassword: string) => {
    await registerAPI(username, email, password, confirmPassword)
      .then((res) => {
        if (res) {
          localStorage.setItem("token", res?.data.token);
          const userObj = {
            username: res?.data.username,
            email: res?.data.email,
          };
          localStorage.setItem("user", JSON.stringify(userObj));
          setToken(res?.data.token!);
          setUser(userObj!);
          axios.defaults.headers.common["Authorization"] = "Bearer " + res?.data.token;
          notifySuccess("Login Success!");
          navigate("/");
        }
      })
      .catch(() => notifyWarning("Server error occurred"));
  };

  const refreshToken = async () => {
    try {
      const res = await axios.post("/api/Account/refresh-token");
      const newToken = res.data.token;
  
      if (!newToken) {
        console.error("Failed to retrieve new token");
        return;
      }
  
      localStorage.setItem("token", newToken);
      setToken(newToken);
      axios.defaults.headers.common["Authorization"] = "Bearer " + newToken;
  
      const decodedToken: any = jwtDecode(newToken);
      const updatedUser = {
        username: decodedToken.given_name,
        email: decodedToken.email,
        roles: decodedToken.role || [],
      };
  
      localStorage.setItem("user", JSON.stringify(updatedUser));
      setUser(updatedUser);
    } catch (error) {
      console.error("Error refreshing token:", error);
      notifyWarning("Failed to refresh token, please log in again.");
      logout(); 
    }
  };
  
  

  const isLoggedIn = () => {
    return !!user;
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
    setToken(null);
    axios.defaults.headers.common["Authorization"] = null;
    navigate("/");
  };

  return (
    <UserContext.Provider
      value={{ user, token, logout, isLoggedIn, registerUser, loginUser, refreshToken }}
    >
      {isReady ? children : null}
    </UserContext.Provider>
  );
};

export const useAuth = () => React.useContext(UserContext);
