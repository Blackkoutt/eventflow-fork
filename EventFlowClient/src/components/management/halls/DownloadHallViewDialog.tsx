import { forwardRef, useEffect } from "react";
import { Hall } from "../../../models/response_models";
import Dialog from "../../common/Dialog";
import RadioButton from "../../common/forms/RadioButton";
import Button, { ButtonStyle } from "../../buttons/Button";
import { faDownload, faXmark } from "@fortawesome/free-solid-svg-icons";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import BlobService from "../../../services/BlobService";

interface DownloadHallDialogProps {
  hall?: Hall;
  onDialogClose: () => void;
}

const DownloadHallViewDialog = forwardRef<HTMLDialogElement, DownloadHallDialogProps>(
  ({ hall, onDialogClose }: DownloadHallDialogProps, ref) => {
    const { data: hallViewData, get: getHallView } = useApi<Blob>(ApiEndpoint.HallView);

    const downloadFile = async () => {
      await getHallView({ id: hall?.id, queryParams: undefined, isBlob: true });
    };

    useEffect(() => {
      if (hallViewData.length !== 0) {
        const fileName = `widok_sali_sala_${hall?.hallNr}.pdf`;
        BlobService.DownloadBlob(hallViewData[0], fileName);
        onDialogClose();
      }
    }, [hallViewData]);

    return (
      <div>
        {hall && (
          <Dialog ref={ref}>
            <div className="flex flex-col justify-center items-center px-5 pb-2 gap-6 max-w-[750px]">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Pobieranie sali</h2>
                <p className="text-textPrimary text-base text-center">
                  Wybierz dane do pobrania dotyczące wybranej sali
                </p>
              </div>
              <div className="flex flex-col justify-start items-start gap-2">
                <RadioButton
                  label="Widok sali (plik pdf)"
                  id="hallViewFileType"
                  value="HallViewPDF"
                  isChecked={true}
                  onChecked={() => {}}
                />
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
          </Dialog>
        )}
      </div>
    );
  }
);

export default DownloadHallViewDialog;
