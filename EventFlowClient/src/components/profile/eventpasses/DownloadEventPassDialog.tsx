import { forwardRef, useEffect, useState } from "react";
import { EventPass } from "../../../models/response_models";
import Dialog from "../../common/Dialog";
import RadioButton from "../../common/forms/RadioButton";
import Button, { ButtonStyle } from "../../buttons/Button";
import { faDownload, faXmark } from "@fortawesome/free-solid-svg-icons";
import { DownloadFileType } from "../../../helpers/enums/DownloadFileType";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import BlobService from "../../../services/BlobService";

interface DownloadEventPassDialogProps {
  eventPass?: EventPass;
  onDialogClose: () => void;
}

const DownloadEventPassDialog = forwardRef<HTMLDialogElement, DownloadEventPassDialogProps>(
  ({ eventPass, onDialogClose }: DownloadEventPassDialogProps, ref) => {
    const { data: JPGData, get: getJPGEventPass } = useApi<Blob>(ApiEndpoint.EventPassJPG);
    const { data: PDFData, get: getPDFEventPass } = useApi<Blob>(ApiEndpoint.EventPassPDF);
    const [checkedValue, setCheckedValue] = useState<string | number>(DownloadFileType.PDF);

    const downloadFile = async () => {
      if (checkedValue === DownloadFileType.PDF) {
        await getPDFEventPass({ id: eventPass?.id, queryParams: undefined, isBlob: true });
      } else if (checkedValue === DownloadFileType.JPG) {
        await getJPGEventPass({ id: eventPass?.id, queryParams: undefined, isBlob: true });
      }
    };

    useEffect(() => {
      if (JPGData.length !== 0) {
        const fileName = `event_flow_karnet_${eventPass?.eventPassGuid}.jpg`;
        BlobService.DownloadBlob(JPGData[0], fileName);
        onDialogClose();
      }
    }, [JPGData]);

    useEffect(() => {
      if (PDFData.length !== 0) {
        const fileName = `event_flow_karnet_${eventPass?.eventPassGuid}.pdf`;
        BlobService.DownloadBlob(PDFData[0], fileName);
        onDialogClose();
      }
    }, [PDFData]);

    return (
      <div>
        {eventPass && (
          <Dialog ref={ref}>
            <div className="flex flex-col justify-center items-center px-5 pb-2 gap-6 max-w-[750px]">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Pobieranie karnetu</h2>
                <p className="text-textPrimary text-base text-center">
                  Wybierz typ pobieranego karnetu
                </p>
              </div>
              <div className="flex flex-col justify-start items-start gap-2">
                <RadioButton
                  label="Plik PDF (karnet JPG + potwierdzenie płatności)"
                  id="pdfFileType"
                  value="PDF"
                  isChecked={true}
                  onChecked={(value) => setCheckedValue(value)}
                />
                <RadioButton
                  label="Plik JPG (karnet JPG)"
                  id="jpgFileType"
                  value="JPG"
                  onChecked={(value) => setCheckedValue(value)}
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

export default DownloadEventPassDialog;
