import { baseUrl } from "../../config/environment/Environment.ts";
import axios, { AxiosInstance } from "axios";
import { ApiUrlConfig } from "../../config/ApiUrlConfig.ts";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum.ts";

export const api: AxiosInstance = axios.create({
  baseURL: baseUrl,
  withCredentials: true,
});

async function Get<TEntity>(
  endpoint: ApiEndpoint,
  queryParams?: Record<string, any>,
  id?: number | string,
  isBlob: boolean = false
) {
  try {
    const url = ApiUrlConfig[endpoint].url(id);
    const queryString = queryParams ? `?${new URLSearchParams(queryParams).toString()}` : "";

    let response;
    if (isBlob) {
      response = await api.get<TEntity>(url + queryString, {
        withCredentials: true,
      });
      response = await api.get<TEntity>(url + queryString, {
        withCredentials: true,
        responseType: "blob",
      });
    } else {
      response = await api.get<TEntity>(url + queryString, {
        withCredentials: true,
      });
    }
    const code = response.status;
    const data = response.data;
    return [data, code];
  } catch (error) {
    throw error;
  }
}

async function Post<TEntity, TPostEntity>(
  endpoint: ApiEndpoint,
  body: TPostEntity,
  id?: number | string
) {
  try {
    const url = ApiUrlConfig[endpoint].url(id);

    let response;
    console.log(body);
    if (body instanceof FormData) {
      response = await api.post<TEntity>(url, body, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
    } else {
      response = await api.post<TEntity>(url, body);
    }
    const code = response.status;
    const data = response.data;
    return [data, code];
  } catch (error) {
    throw error;
  }
}

async function Put<TEntity, TPutEntity>(
  endpoint: ApiEndpoint,
  body: TPutEntity,
  id?: number | string
) {
  try {
    const url = ApiUrlConfig[endpoint].url(id);

    let response;
    if (body instanceof FormData) {
      response = await api.put<TEntity>(url, body, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
    } else {
      response = await api.put<TEntity>(url, body);
    }
    const code = response.status;
    const data = response.data;
    return [data, code];
  } catch (error) {
    throw error;
  }
}

async function Patch<TEntity, TPatchEntity>(
  endpoint: ApiEndpoint,
  body?: TPatchEntity,
  id?: number | string
) {
  try {
    const url = ApiUrlConfig[endpoint].url(id);
    const response = await api.patch<TEntity>(url, body);

    const code = response.status;
    const data = response.data;
    return [data, code];
  } catch (error) {
    throw error;
  }
}

async function Delete<TEntity>(endpoint: ApiEndpoint, id: number | string) {
  try {
    const url = ApiUrlConfig[endpoint].url(id);

    const response = await api.delete<TEntity>(url);
    const code = response.status;
    const data = response.data;
    return [data, code];
  } catch (error) {
    throw error;
  }
}

const GetPhotoEndpoint = (photoEndpoint?: string): string => {
  return photoEndpoint ? `${baseUrl}${photoEndpoint}` : "";
};

type BlobResponse = {
  code: number;
  data: Blob;
};
const GetPhoto = async (photoEndpoint?: string) => {
  try {
    const fullURL = photoEndpoint ? `${baseUrl}${photoEndpoint}` : "";
    const response = await api.get<Blob>(fullURL, {
      withCredentials: true,
      responseType: "blob",
    });

    const code = response.status;
    const data = response.data;
    return { code: code, data: data } as BlobResponse;
  } catch (error) {
    throw error;
  }
};

const ApiMethod = {
  Get,
  GetPhotoEndpoint,
  GetPhoto,
  Post,
  Put,
  Patch,
  Delete,
};

export default ApiMethod;
