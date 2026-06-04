import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5199',
  withCredentials: true, 
});

let accessToken = '';
let userId = '';

export const getUserId = () => {
  return userId;
}

const setUserId = (uId : string) => {
  userId = uId;
}

export const setAccessToken = (token: string) => {
  accessToken = token;
};

export const getAccessToken = async () => {
  if (accessToken.length<=0)
  {
    const result = await api.post('/api/LoginRegister/refresh');
    setUserId(result.data.id);
    if (result.status == 401)
    {
        location.href='/login';
        return;
    }
    return result.data.token;
  }
  setAccessToken(accessToken);
  return accessToken;
}

api.interceptors.request.use(
  (config) => {
    if (accessToken) {
      config.headers['Authorization'] = `Bearer ${accessToken}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const response = await axios.post('http://localhost:5199/api/LoginRegister/refresh', {}, { withCredentials: true });
        const newAccessToken = response.data.token;
        setUserId(response.data.id);
        setAccessToken(newAccessToken);
        originalRequest.headers['Authorization'] = `Bearer ${newAccessToken}`;
        return api(originalRequest); 
      } catch (refreshError) {
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error);
  }
);

export default api;

export async function checkAuthWithBackend() {
  if (accessToken.length > 0)
    return true;
  try {
    await api.get('/api/User/getUserData');
    return true;
  } catch (e) {
    return false;
  }
}