import axios from "axios";

export const setupAxiosInterceptors = () => {
  axios.interceptors.request.use(
    (config) => {
      const token = localStorage.getItem("token");
      if (token) {
        config.headers["Authorization"] = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      console.error("Error in request:", error);
      return Promise.reject(error);
    }
  );

  axios.interceptors.response.use(
    (response) => response,
    async (error) => {
      const originalRequest = error.config;

      if (error.response?.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;

        try {
          const { data } = await axios.post("/refresh-token");

          const newToken = data.Token;
          localStorage.setItem("token", newToken);

          axios.defaults.headers.common["Authorization"] = `Bearer ${newToken}`;
          originalRequest.headers["Authorization"] = `Bearer ${newToken}`;

          return axios(originalRequest);
        } catch (refreshError) {
          console.error("Error during token update:", refreshError);
          localStorage.removeItem("token");
          localStorage.removeItem("user");
          window.location.href = "/login"; 
          return Promise.reject(refreshError);
        }
      }

      return Promise.reject(error);
    }
  );
};
