import { DataTable } from "primereact/datatable";
import useApi from "../../hooks/useApi";
import { AdditionalServices, HallRent } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import { useEffect, useRef, useState } from "react";
import { Column, ColumnBodyOptions } from "primereact/column";
import DateFormatter from "../../helpers/DateFormatter";
import { DateFormat } from "../../helpers/enums/DateFormatEnum";
import StatusBody from "../../components/tabledata/StatusBody";
import { Status } from "../../helpers/enums/Status";
import TableActionButton from "../../components/buttons/TableActionButton";
import {
  faDownload,
  faInfoCircle,
  faPenToSquare,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import DownloadHallRentDialog from "../../components/profile/hallrents/DownloadHallRentDialog";
import DetailsHallRentDialog from "../../components/profile/hallrents/DetailsHallRentDialog";
import CancelHallRentDialog from "../../components/profile/hallrents/CancelHallRentDialog";
import ModifyHallDialog from "../../components/profile/hallrents/ModifyHallDialog";

const UserHallRents = () => {
  const { data: hallRents, get: getHallRents } = useApi<HallRent>(ApiEndpoint.HallRent);

  const downloadHallRentDialog = useRef<HTMLDialogElement>(null);
  const detailsHallRentDialog = useRef<HTMLDialogElement>(null);
  const cancelHallRentDialog = useRef<HTMLDialogElement>(null);
  const modifyHallDialog = useRef<HTMLDialogElement>(null);

  const [hallRentToDownload, setHallRentToDownload] = useState<HallRent | undefined>(undefined);
  const [hallRentToDetails, setHallRentToDetails] = useState<HallRent | undefined>(undefined);
  const [hallRentToCancel, setHallRentToCancel] = useState<HallRent | undefined>(undefined);
  const [hallRentToModifyHall, setHallRentToModifyHall] = useState<HallRent | undefined>(undefined);

  useEffect(() => {
    getHallRents({ id: undefined, queryParams: undefined });
  }, []);

  useEffect(() => {
    if (hallRentToDownload !== undefined) {
      downloadHallRentDialog.current?.showModal();
    }
  }, [hallRentToDownload]);

  useEffect(() => {
    if (hallRentToCancel !== undefined) {
      cancelHallRentDialog.current?.showModal();
    }
  }, [hallRentToCancel]);

  useEffect(() => {
    if (hallRentToDetails !== undefined) {
      detailsHallRentDialog.current?.showModal();
    }
  }, [hallRentToDetails]);

  useEffect(() => {
    console.log(hallRentToModifyHall);
    if (hallRentToModifyHall !== undefined) {
      modifyHallDialog.current?.showModal();
    }
  }, [hallRentToModifyHall]);

  const actionsTemplate = (rowData: HallRent, _options: ColumnBodyOptions) => {
    return (
      <div className="flex flex-row justify-start items-start gap-3">
        {rowData.hallRentStatus === Status.Active && (
          <TableActionButton
            icon={faPenToSquare}
            buttonColor="#22c55e"
            text="Modyfikuj salę"
            width={160}
            onClick={() => {
              setHallRentToModifyHall(rowData);
              modifyHallDialog.current?.showModal();
            }}
          />
        )}
        <TableActionButton
          icon={faInfoCircle}
          buttonColor="#f97316"
          text="Szczegóły"
          onClick={() => {
            setHallRentToDetails(rowData);
            detailsHallRentDialog.current?.showModal();
          }}
        />
        {rowData.hallRentStatus !== Status.Canceled && (
          <TableActionButton
            icon={faDownload}
            buttonColor="#0ea5e9"
            text="Pobierz"
            width={120}
            onClick={() => {
              setHallRentToDownload(rowData);
              downloadHallRentDialog.current?.showModal();
            }}
          />
        )}
        {rowData.hallRentStatus === Status.Active && (
          <TableActionButton
            icon={faTrash}
            buttonColor="#ef4444"
            text="Anuluj"
            width={100}
            onClick={() => {
              setHallRentToCancel(rowData);
              cancelHallRentDialog.current?.showModal();
            }}
          />
        )}
      </div>
    );
  };

  const reloadHallRentsAfterSuccessDialog = () => {
    cancelHallRentDialog.current?.close();
    modifyHallDialog.current?.close();
    setHallRentToCancel(undefined);
    setHallRentToModifyHall(undefined);
    getHallRents({ id: undefined, queryParams: undefined });
  };

  return (
    <div className="max-w-[54vw] self-center">
      <DownloadHallRentDialog
        ref={downloadHallRentDialog}
        hallRent={hallRentToDownload}
        onDialogClose={() => downloadHallRentDialog.current?.close()}
      />

      <ModifyHallDialog
        ref={modifyHallDialog}
        hallId={hallRentToModifyHall?.hall?.id}
        hallRentId={hallRentToModifyHall?.id}
        onDialogClose={() => modifyHallDialog.current?.close()}
        onDialogConfirm={reloadHallRentsAfterSuccessDialog}
      />

      <CancelHallRentDialog
        ref={cancelHallRentDialog}
        hallRent={hallRentToCancel}
        onDialogClose={() => cancelHallRentDialog.current?.close()}
        onDialogConfirm={reloadHallRentsAfterSuccessDialog}
      />

      <DetailsHallRentDialog ref={detailsHallRentDialog} hallRent={hallRentToDetails} />

      <DataTable
        value={hallRents}
        paginator
        removableSort
        rows={5}
        rowsPerPageOptions={[5, 10, 25, 50]}
        stripedRows
        showGridlines
      >
        <Column field="id" sortable header="ID" />
        <Column
          field="startDate"
          sortable
          header="Data rozpoczęcia"
          body={(rowData) => DateFormatter.FormatDate(rowData.startDate, DateFormat.DateTime)}
        />
        <Column
          field="endDate"
          sortable
          header="Data zakończenia"
          body={(rowData) => DateFormatter.FormatDate(rowData.endDate, DateFormat.DateTime)}
        />
        <Column field="hall.hallNr" sortable header="Nr sali" />
        <Column field="hall.rentalPricePerHour" sortable header="Cena za 1h (zł)" />

        <Column
          field="additionalServices"
          header="Dodatkowe usługi"
          body={(rowData) =>
            rowData.additionalServices
              .map((service: AdditionalServices) => `${service.name} (${service.price} zł)`)
              .join(", ")
          }
        />
        <Column field="paymentAmount" sortable header="Koszt (zł)" />
        <Column
          header="Status"
          body={(rowData) => (
            <StatusBody
              status={rowData.hallRentStatus}
              activeStatusText="Aktywna rezerwacja sali"
              expiredStatusText="Zakończona rezerwacja sali"
              canceledStatusText="Anulowana rezerwacja sali"
              unknownStatusText="Nieznany status"
            />
          )}
        />
        <Column header="Akcja" body={actionsTemplate} />
      </DataTable>
    </div>
  );
};
export default UserHallRents;
