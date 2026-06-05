import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5199',
  withCredentials: true, 
});

let accessToken = '';
let userId = '';

let isRefreshing = false;
let isInitialRefreshing = false; 
let initialRefreshPromise: Promise<string | null> | null = null; 
let failedQueue: any[] = [];

const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) prom.reject(error);
    else prom.resolve(token);
  });
  failedQueue = [];
};

export const getUserId = () => userId;
const setUserId = (uId: string) => { userId = uId; };
export const setAccessToken = (token: string) => { accessToken = token; };

export const getAccessToken = async () => {
  if (accessToken.length > 0) {
    return accessToken;
  }

  if (isInitialRefreshing && initialRefreshPromise) {
    return initialRefreshPromise;
  }

  isInitialRefreshing = true;

  initialRefreshPromise = (async () => {
    try {
      const result = await axios.post('http://localhost:5199/api/LoginRegister/refresh', {}, { withCredentials: true });
      setUserId(result.data.id);
      setAccessToken(result.data.token);
      return result.data.token;
    } catch (error) {
      setAccessToken('');
      window.location.href = '/login';
      return null;
    } finally {
      isInitialRefreshing = false;
      initialRefreshPromise = null;
    }
  })();

  return initialRefreshPromise;
};

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
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers['Authorization'] = `Bearer ${token}`;
            return api(originalRequest);
          })
          .catch((err) => Promise.reject(err));
      }

      originalRequest._retry = true;
      isRefreshing = true;

      return new Promise((resolve, reject) => {
        axios.post('http://localhost:5199/api/LoginRegister/refresh', {}, { withCredentials: true })
          .then((response) => {
            const newAccessToken = response.data.token;
            setUserId(response.data.id);
            setAccessToken(newAccessToken);
            
            originalRequest.headers['Authorization'] = `Bearer ${newAccessToken}`;
            processQueue(null, newAccessToken);
            resolve(api(originalRequest));
          })
          .catch((refreshError) => {
            processQueue(refreshError, null);
            setAccessToken('');
            window.location.href = '/login';
            reject(refreshError);
          })
          .finally(() => {
            isRefreshing = false;
          });
      });
    }
    
    return Promise.reject(error);
  }
);

export default api;

export async function checkAuthWithBackend() {
  if (accessToken.length > 0) return true;
  
  try {
    const token = await getAccessToken();
    return !!token;
  } catch (e) {
    return false;
  }
}