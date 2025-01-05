import axios from "axios";
import { UserProfileToken } from "../models/User";
import { handleError } from "../helpers/ErrorHandler";

export const loginAPI = async (email: string, password: string) => {
  try {
    const data = await axios.post<UserProfileToken>("api/account/login", {
      email: email,
      password: password,
    });
    return data; 
  } catch (error) {
    handleError(error);
  }
};

export const registerAPI = async ( username: string, email: string, password: string, confirmPassword: string) => {
  try {
    const data = await axios.post<UserProfileToken>("api/account/register", {
      username: username,
      email: email,
      password: password,
      ConfirmPassword: confirmPassword,
    });
    return data;
  } catch (error) {
    handleError(error);
  }
};