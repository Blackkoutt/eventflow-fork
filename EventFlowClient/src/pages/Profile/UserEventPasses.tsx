import { DataTable } from "primereact/datatable";
import { EventPass } from "../../models/response_models";
import useApi from "../../hooks/useApi";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import { useEffect, useRef, useState } from "react";
import { Column, ColumnBodyOptions } from "primereact/column";
import TableActionButton from "../../components/buttons/TableActionButton";
import { faClock, faDownload, faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { Status } from "../../helpers/enums/Status";
import DateFormatter from "../../helpers/DateFormatter";
import { DateFormat } from "../../helpers/enums/DateFormatEnum";
import StatusBody from "../../components/tabledata/StatusBody";
import DetailsEventPassDialog from "../../components/profile/eventpasses/DetailsEventPassDialog";
import DownloadEventPassDialog from "../../components/profile/eventpasses/DownloadEventPassDialog";
import RenewEventPassDialog from "../../components/profile/eventpasses/RenewEventPassDialog";

const UserEventPasses = () => {
  const { data: eventPasses, get: getEventPasses } = useApi<EventPass>(ApiEndpoint.EventPass);

  const detailsEventPassDialog = useRef<HTMLDialogElement>(null);
  const downloadEventPassDialog = useRef<HTMLDialogElement>(null);
  const renewEventPassDialog = useRef<HTMLDialogElement>(null);

  const [eventPassToDetails, setEventPassToDetails] = useState<EventPass | undefined>(undefined);
  const [eventPassToDownload, setEventPassToDownload] = useState<EventPass | undefined>(undefined);
  const [eventPassToRenew, setEventPassToRenew] = useState<EventPass | undefined>(undefined);

  useEffect(() => {
    if (eventPassToDownload != undefined) {
      downloadEventPassDialog.current?.showModal();
    }
  }, [eventPassToDownload]);

  useEffect(() => {
    if (eventPassToDetails != undefined) {
      detailsEventPassDialog.current?.showModal();
    }
  }, [eventPassToDetails]);

  useEffect(() => {
    if (eventPassToRenew != undefined) {
      renewEventPassDialog.current?.showModal();
    }
  }, [eventPassToRenew]);

  useEffect(() => {
    getEventPasses({ id: undefined, queryParams: undefined });
  }, []);

  useEffect(() => {
    console.log(eventPasses);
  }, [eventPasses]);

  const actionsTemplate = (rowData: EventPass, _options: ColumnBodyOptions) => {
    return (
      <div className="flex flex-row justify-start items-start gap-3">
        {rowData.eventPassStatus === Status.Active && (
          <TableActionButton
            icon={faClock}
            buttonColor="#22c55e"
            text="Przedłuż karnet"
            width={160}
            onClick={() => {
              setEventPassToRenew(rowData);
              renewEventPassDialog.current?.showModal();
            }}
          />
        )}
        <TableActionButton
          icon={faInfoCircle}
          buttonColor="#f97316"
          text="Szczegóły"
          onClick={() => {
            setEventPassToDetails(rowData);
            detailsEventPassDialog.current?.showModal();
          }}
        />
        {rowData.eventPassStatus !== Status.Canceled && (
          <TableActionButton
            icon={faDownload}
            buttonColor="#0ea5e9"
            text="Pobierz karnet"
            width={160}
            onClick={() => {
              setEventPassToDownload(rowData);
              downloadEventPassDialog.current?.showModal();
            }}
          />
        )}
      </div>
    );
  };

  const reloadEventPassesAfterSuccessDialog = () => {
    renewEventPassDialog.current?.close();
    setEventPassToRenew(undefined);
    getEventPasses({ id: undefined, queryParams: undefined });
  };

  return (
    <div className="max-w-[54vw] self-center">
      <DetailsEventPassDialog ref={detailsEventPassDialog} eventPass={eventPassToDetails} />

      <RenewEventPassDialog
        ref={renewEventPassDialog}
        eventPass={eventPassToRenew}
        onDialogClose={() => renewEventPassDialog.current?.close()}
        onDialogConfirm={reloadEventPassesAfterSuccessDialog}
      />

      <DownloadEventPassDialog
        ref={downloadEventPassDialog}
        eventPass={eventPassToDownload}
        onDialogClose={() => downloadEventPassDialog.current?.close()}
      />

      <DataTable
        value={eventPasses}
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
          header="Data zakupu"
          body={(rowData) => DateFormatter.FormatDate(rowData.startDate, DateFormat.Date)}
        />
        <Column
          field="renewalDate"
          sortable
          header="Data przedłużenia"
          body={(rowData) => DateFormatter.FormatDate(rowData.renewalDate, DateFormat.Date)}
        />
        <Column
          field="endDate"
          sortable
          header="Data zakończenia"
          body={(rowData) => DateFormatter.FormatDate(rowData.endDate, DateFormat.Date)}
        />
        <Column field="passType.name" sortable header="Typ karnetu" />
        <Column field="paymentAmount" sortable header="Koszt (zł)" />

        <Column
          header="Status"
          body={(rowData) => (
            <StatusBody
              status={rowData.eventPassStatus}
              activeStatusText="Aktywny karnet"
              expiredStatusText="Przeterminowany karnet"
              canceledStatusText="Anulowany karnet"
              unknownStatusText="Nieznany status"
            />
          )}
        />
        <Column header="Akcja" body={actionsTemplate} />
      </DataTable>
    </div>
  );
};
export default UserEventPasses;
