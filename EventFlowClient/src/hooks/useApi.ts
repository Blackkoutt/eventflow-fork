import { useState, useCallback } from "react";
import ApiClient, { api } from "../services/api/ApiClientService";
import { ApiEndpoint } from "../helpers/enums/ApiEndpointEnum";
import { HTTPMethod } from "../helpers/enums/HTTPMethodEnum";
import { AxiosError } from "axios";
import { APIError } from "../models/error/APIError";
import { toast } from "react-toastify";

interface RequestParams<TPostEntity, TPutEntity, TPatchEntity> {
  httpMethod: HTTPMethod;
  id?: number | string;
  body?: TPostEntity | TPutEntity | TPatchEntity;
  queryParams?: Record<string, any>;
  isBlob?: boolean;
}

interface GETRequestParams {
  id?: number | string;
  queryParams?: Record<string, any>;
  isBlob?: boolean;
}
interface POSTRequestParams<TPostEntity> {
  body: TPostEntity;
  id?: number;
}
interface PUTRequestParams<TPutEntity> {
  body: TPutEntity;
  id?: number;
}
interface PATCHRequestParams<TPatchEntity> {
  id?: number;
  body?: TPatchEntity;
}
interface DELETERequestParams {
  id: number;
}

function useApi<TEntity, TPostEntity = undefined, TPutEntity = undefined, TPatchEntity = undefined>(
  endpoint: ApiEndpoint
) {
  const [data, setData] = useState<TEntity[]>([]);
  const [statusCode, setStatusCode] = useState<number | null>(null);
  const [error, setError] = useState<APIError | null>(null);
  const [loading, setLoading] = useState(false);

  const request = useCallback(
    async ({
      httpMethod,
      id,
      body,
      queryParams,
      isBlob = false,
    }: RequestParams<TPostEntity, TPutEntity, TPatchEntity>) => {
      setLoading(true);
      setError(null);
      // Status code of the finished request, returned so callers can react to it right away
      let resultCode: number | null = null;
      try {
        switch (httpMethod) {
          case HTTPMethod.GET:
            const [getData, getCode] = await ApiClient.Get<TEntity[]>(
              endpoint,
              queryParams,
              id,
              isBlob
            );
            let dataArray: TEntity[] = getData as TEntity[];
            if (!Array.isArray(dataArray)) dataArray = [getData as TEntity];
            setData(dataArray);
            resultCode = getCode as number;
            setStatusCode(resultCode);
            break;

          case HTTPMethod.POST:
            // if (typeof body === undefined || body === undefined)
            //   throw Error("POST Error: body is undefined");
            const [postData, postCode] = await ApiClient.Post<TEntity, TPostEntity>(
              endpoint,
              body as TPostEntity,
              id
            );

            console.log(postCode);
            /* setData((prev) => {
              if (Array.isArray(prev)) {
                return [...prev, postData as TEntity];
              } else {
                return postData as TEntity;
              }
            });*/
            setData((prev) => [...prev, postData as TEntity]);
            resultCode = postCode as number;
            setStatusCode(resultCode);
            break;

          case HTTPMethod.PUT:
            // if (typeof body === undefined || body === undefined)
            //   throw Error("PUT Error: body is undefined");
            const [putData, putCode] = await ApiClient.Put<TEntity, TPutEntity>(
              endpoint,
              body as TPutEntity,
              id
            );
            console.log("putcode", putCode);
            resultCode = putCode as number;
            setStatusCode(resultCode);
            break;

          case HTTPMethod.PATCH:
            console.log("heloov2");
            console.log("endpoint", endpoint);
            const [patchData, patchCode] = await ApiClient.Patch<TEntity, TPatchEntity>(
              endpoint,
              body as TPatchEntity | undefined,
              id
            );
            console.log("patchCode", patchCode);
            /* setData((prev) =>
                prev.map((item) => (item.id === id ? { ...item, ...putResponse } : item))
              );*/
            resultCode = patchCode as number;
            setStatusCode(resultCode);
            break;

          case HTTPMethod.DELETE:
            if (id === undefined) throw Error("DELETE Error: id is undefined");
            const [deleteData, deleteCode] = await ApiClient.Delete(endpoint, id);
            //setData((prev) => prev.filter((item) => item.id !== id));
            resultCode = deleteCode as number;
            setStatusCode(resultCode);
            break;
          default:
            throw new Error(`Unsupported HTTP method: ${httpMethod}`);
        }
      } catch (caughtError) {
        if (caughtError instanceof AxiosError) {
          const responseData = caughtError.response?.data || {};
          console.log(caughtError);
          const apiError: APIError = {
            code: responseData.Code || responseData.code,
            title: responseData.Title || responseData.title,
            type: responseData.Type || responseData.type,
            details: responseData.Details || responseData.details,
          };

          console.log("API Error: ", apiError);
          setError(apiError);
          resultCode = apiError.code;
          setStatusCode(resultCode);
          if (apiError.details.errors.length > 0) {
            apiError.details.errors.forEach((error) => {
              toast.error(error);
            });
          }
          //throw caughtError;
        }
      } finally {
        setLoading(false);
      }
      return resultCode;
    },
    [endpoint]
  );

  const get = useCallback(
    ({ id, queryParams, isBlob }: GETRequestParams): Promise<number | null> =>
      request({ httpMethod: HTTPMethod.GET, id, queryParams, isBlob }),
    [request]
  );

  const post = useCallback(
    ({ body, id }: POSTRequestParams<TPostEntity>): Promise<number | null> =>
      request({ httpMethod: HTTPMethod.POST, id, body }),
    [request]
  );

  const put = useCallback(
    ({ body, id }: PUTRequestParams<TPutEntity>): Promise<number | null> =>
      request({ httpMethod: HTTPMethod.PUT, id, body }),
    [request]
  );

  const patch = useCallback(
    ({ id, body }: PATCHRequestParams<TPatchEntity>): Promise<number | null> =>
      request({ httpMethod: HTTPMethod.PATCH, id, body }),
    [request]
  );

  const del = useCallback(
    ({ id }: DELETERequestParams): Promise<number | null> => request({ httpMethod: HTTPMethod.DELETE, id }),
    [request]
  );

  return { data, error, statusCode, loading, get, post, put, patch, del };
}

export default useApi;

// function useApi<TEntity, TPostEntity=undefined, TPutEntity=undefined>(endpoint: ApiEndpoint) {
//   const [data, setData] = useState<TEntity[]>([]);
//   const [error, setError] = useState<string | null>(null);
//   const [loading, setLoading] = useState(false);

//   const request = useCallback(
//     async ({ httpMethod, id, body, queryParams }: RequestParams<TPostEntity, TPutEntity>) => {
//       setLoading(true);
//       setError(null);
//       try {
//         let response: TEntity[] = [];
//         switch (httpMethod) {
//           case HTTPMethod.GET:
//             response = await ApiClient.Get<TEntity[]>(endpoint, queryParams);
//             setData(response);
//             break;
//           case HTTPMethod.POST:
//             if()
//         }
//         console.log(
//           `${HTTPMethod[httpMethod]} ${ApiEndpoint[endpoint]}${id ? `/${id}` : ""}:`,
//           response
//         );
//       } catch (error) {
//         if (error instanceof Error) {
//           setError(`${HTTPMethod[httpMethod]} ${ApiEndpoint[endpoint]}${id ? `/${id}` : ""}: ${error.message}`);
//         } else {
//           setError("Something went wrong");
//         }
//         console.log("Error: ", error);
//       } finally {
//         setLoading(false);
//       }
//     },
//     [endpoint]
//   );

//   const get = ({ id, queryParams }: GETRequestParams): Promise<void> =>
//     request({ httpMethod: HTTPMethod.GET, id: id, queryParams: queryParams });

//   const post = <TPostEntity>({ body }: POSTRequestParams<TPostEntity>): Promise<void> =>
//     request({ httpMethod: HTTPMethod.POST, body: body });

//   const put = ({ id, body }: PUTRequestParams<TEntity>): Promise<void> =>
//     request({ httpMethod: HTTPMethod.PUT, id: id, body: body });

//   const del = ({ id }: DELETERequestParams): Promise<void> =>
//     request({ httpMethod: HTTPMethod.DELETE, id: id });

//   return { data, error, loading, get, post, put, del };
// }

// export default useApi;
