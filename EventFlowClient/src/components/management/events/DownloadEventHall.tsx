import { forwardRef, useEffect } from "react";
import { EventEntity } from "../../../models/response_models";
import Button, { ButtonStyle } from "../../buttons/Button";
import { faDownload, faXmark } from "@fortawesome/free-solid-svg-icons";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import BlobService from "../../../services/BlobService";

interface DownloadEventHallProps {
  item?: EventEntity;
  onDialogClose: () => void;
}

const DownloadEventHall = forwardRef<HTMLDialogElement, DownloadEventHallProps>(
  ({ item, onDialogClose }: DownloadEventHallProps, _ref) => {
    const { data: hallViewData, get: getHallView } = useApi<Blob>(ApiEndpoint.EventHallView);

    const downloadFile = async () => {
      await getHallView({ id: item?.id, queryParams: undefined, isBlob: true });
    };

    useEffect(() => {
      if (hallViewData.length !== 0) {
        const fileName = `widok_sali_wydarzenie_${item?.id}.pdf`;
        BlobService.DownloadBlob(hallViewData[0], fileName);
        onDialogClose();
      }
    }, [hallViewData]);

    return (
      <div className="py-3">
        {item && (
          <div className="flex flex-col justify-center items-center px-5 pb-2 gap-6 max-w-[750px]">
            <div className="flex flex-col justify-center items-center gap-2">
              <h2>Pobieranie widoku sali</h2>
              <p className="text-textPrimary text-base text-center">
                Pobierz widok sali wydarzenia {item.title} w formie PDF.
              </p>
            </div>
            <div className="flex flex-row justify-center items-center gap-2">
              <Button
                text="Anuluj"
                width={145}
                height={45}
                icon={faXmark}
                iconSize={18}
                style={ButtonStyle.DefaultGray}
                action={onDialogClose}
              ></Button>
              <Button
                text="Pobierz"
                width={145}
                height={45}
                icon={faDownload}
                iconSize={18}
                style={ButtonStyle.Primary}
                action={downloadFile}
              ></Button>
            </div>
          </div>
        )}
      </div>
    );
  }
);

export default DownloadEventHall;
