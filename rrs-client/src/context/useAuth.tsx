
import { createContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import React from "react";
import axios from "axios";
import { UserProfile } from "../models/User";
import { loginAPI, registerAPI } from "../services/AuthService";
import { notifySuccess, notifyWarning } from "../utils/toastUtils";

type UserContextType = {
  user: UserProfile | null;
  token: string | null;
  registerUser: ( username: string, email: string, password: string) => void;
  loginUser: (email: string, password: string) => void;
  logout: () => void;
  isLoggedIn: () => boolean;
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
      .catch(() => notifyWarning("Server error occured"));
  };
  
  const registerUser = async (username: string, email: string, password: string) => {
    await registerAPI(username, email, password)
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
      .catch(() => notifyWarning("Server error occured"));
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
      value={{ user, token, logout, isLoggedIn, registerUser, loginUser }}
    >
      {isReady ? children : null}
    </UserContext.Provider>
  );
};

export const useAuth = () => React.useContext(UserContext);
