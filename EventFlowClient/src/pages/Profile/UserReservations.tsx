import { useEffect, useRef, useState } from "react";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { Reservation, Seat } from "../../models/response_models";
import { DataTable } from "primereact/datatable";
import { Column, ColumnBodyOptions } from "primereact/column";
import DateFormatter from "../../helpers/DateFormatter";
import { DateFormat } from "../../helpers/enums/DateFormatEnum";
import { Status } from "../../helpers/enums/Status";
import { faDownload, faInfoCircle, faTrash } from "@fortawesome/free-solid-svg-icons";
import TableActionButton from "../../components/buttons/TableActionButton";
import DetailsReservationDialog from "../../components/profile/reservations/DetailsReservationDialog";
import DownloadReservationTicketDialog from "../../components/profile/reservations/DownloadReservationTicketDialog";
import StatusBody from "../../components/tabledata/StatusBody";
import CancelReservationDialog from "../../components/profile/reservations/CancelReservationDialog";

const UserReservations = () => {
  const { data: reservations, get: getReservations } = useApi<Reservation>(ApiEndpoint.Reservation);
  const cancelReservationDialog = useRef<HTMLDialogElement>(null);
  const detailsReservationDialog = useRef<HTMLDialogElement>(null);
  const downloadReservationDialog = useRef<HTMLDialogElement>(null);

  console.log(reservations);

  const [reservationToCancel, setReservationToCancel] = useState<Reservation | undefined>(
    undefined
  );
  const [reservationToDetails, setReservationToDetails] = useState<Reservation | undefined>(
    undefined
  );
  const [reservationToDownload, setReservationToDownload] = useState<Reservation | undefined>(
    undefined
  );

  useEffect(() => {
    if (reservationToCancel != undefined) {
      cancelReservationDialog.current?.showModal();
    }
  }, [reservationToCancel]);

  useEffect(() => {
    if (reservationToDetails != undefined) {
      detailsReservationDialog.current?.showModal();
    }
  }, [reservationToDetails]);

  useEffect(() => {
    if (reservationToDownload != undefined) {
      downloadReservationDialog.current?.showModal();
    }
  }, [reservationToDownload]);

  useEffect(() => {
    getReservations({ id: undefined, queryParams: undefined });
  }, []);

  const reloadReservationsAfterSuccessDialog = () => {
    cancelReservationDialog.current?.close();
    setReservationToCancel(undefined);
    getReservations({ id: undefined, queryParams: undefined });
  };

  const actionsTemplate = (rowData: Reservation, _options: ColumnBodyOptions) => {
    return (
      <div className="flex flex-row justify-start items-start gap-3">
        <TableActionButton
          icon={faInfoCircle}
          buttonColor="#f97316"
          text="Szczegóły"
          onClick={() => {
            setReservationToDetails(rowData);
            detailsReservationDialog.current?.showModal();
          }}
        ></TableActionButton>
        {rowData.reservationStatus !== Status.Canceled && (
          <TableActionButton
            icon={faDownload}
            buttonColor="#0ea5e9"
            text="Pobierz bilet"
            width={150}
            onClick={() => {
              setReservationToDownload(rowData);
              downloadReservationDialog.current?.showModal();
            }}
          ></TableActionButton>
        )}
        {rowData.reservationStatus === Status.Active && (
          <TableActionButton
            icon={faTrash}
            buttonColor="#ef4444"
            text="Anuluj"
            onClick={() => {
              setReservationToCancel(rowData);
              cancelReservationDialog.current?.showModal();
            }}
          ></TableActionButton>
        )}
      </div>
    );
  };

  return (
    <div className="max-w-[54vw] self-center">
      <CancelReservationDialog
        ref={cancelReservationDialog}
        reservation={reservationToCancel}
        onDialogClose={() => cancelReservationDialog.current?.close()}
        onDialogConfirm={reloadReservationsAfterSuccessDialog}
      />

      <DetailsReservationDialog ref={detailsReservationDialog} reservation={reservationToDetails} />

      <DownloadReservationTicketDialog
        ref={downloadReservationDialog}
        reservation={reservationToDownload}
        onDialogClose={() => downloadReservationDialog.current?.close()}
      />

      <DataTable
        value={reservations}
        paginator
        removableSort
        rows={5}
        rowsPerPageOptions={[5, 10, 25, 50]}
        stripedRows
        showGridlines
      >
        <Column field="id" sortable header="ID"></Column>
        <Column
          field="reservationDate"
          sortable
          header="Data rezerwacji"
          body={(rowData) => DateFormatter.FormatDate(rowData.reservationDate, DateFormat.DateTime)}
        ></Column>
        <Column field="ticket.event.title" sortable header="Wydarzenie" />
        <Column field="ticket.festival.title" sortable header="Festiwal" />

        <Column field="ticket.event.hall.hallNr" header="Nr sali" />
        <Column
          field="seats"
          header="Nr Miejsc"
          body={(rowData) => rowData.seats.map((seat: Seat) => seat.seatNr).join(", ")}
        />
        <Column field="paymentAmount" sortable header="Koszt (zł)"></Column>
        <Column
          header="Status"
          body={(rowData) => (
            <StatusBody
              status={rowData.reservationStatus}
              activeStatusText="Aktywna rezerwacja"
              expiredStatusText="Zakończona rezerwacja"
              canceledStatusText="Anulowana rezerwacja"
              unknownStatusText="Nieznany status"
            />
          )}
        ></Column>
        <Column header="Akcja" body={actionsTemplate}></Column>
      </DataTable>
    </div>
  );
};
export default UserReservations;
