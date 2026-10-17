import { forwardRef, useEffect, useState } from "react";
import { Hall, Seat, SeatType } from "../../../models/response_models";
import Dialog from "../../common/Dialog";
import Button, { ButtonStyle } from "../../buttons/Button";
import { faCouch, faPenToSquare, faXmark } from "@fortawesome/free-solid-svg-icons";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ContextMenu from "../../common/ContextMenu";
import LabelText from "../../common/LabelText";
import MessageText from "../../common/MessageText";
import { MessageType } from "../../../helpers/enums/MessageTypeEnum";
import { SeatRequest, SeatSchema } from "../../../models/create_schemas/SeatSchema";
import { toast } from "react-toastify";
import { HallRentHallUpdateRequest } from "../../../models/update_schemas/HallRentHallUpdateSchema";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";

interface ModifyHallDialogProps {
  hallId?: number;
  hallRentId?: number;
  onDialogClose: () => void;
  onDialogConfirm: () => void;
}

interface ContextMenuState {
  selectedSeatNr?: number;
  position: { x: number; y: number };
  items: { label: string; action: (selectedSeatNr?: number) => void }[];
}

const ModifyHallDialog = forwardRef<HTMLDialogElement, ModifyHallDialogProps>(
  ({ hallId, hallRentId, onDialogClose, onDialogConfirm }: ModifyHallDialogProps, ref) => {
    const { data: hallWithDetails, get: getHallWithDetails } = useApi<Hall>(ApiEndpoint.Hall);
    const { statusCode: statusCode, put: updateHall } = useApi<
      Hall,
      undefined,
      HallRentHallUpdateRequest
    >(ApiEndpoint.HallRentUpdateHall);
    const { data: seatTypes, get: getSeatTypes } = useApi<SeatType>(ApiEndpoint.SeatType);
    //const [selectedSeatNr, setSelectedSeatNr] = useState<number | undefined>(undefined);
    const [hallSeats, setHallSeats] = useState<Seat[]>([]);
    const [contextMenu, setContextMenu] = useState<ContextMenuState | null>(null);
    const [actionPerformed, setActionPerformed] = useState(false);
    const [promisePending, setPromisePending] = useState(false);

    useEffect(() => {
      if (hallId !== undefined) {
        getHallWithDetails({ id: hallId, queryParams: undefined });
        getSeatTypes({ id: undefined, queryParams: undefined });
      }
    }, [hallId]);

    useEffect(() => {
      console.log(hallWithDetails);
      if (hallWithDetails !== undefined && hallWithDetails.length !== 0) {
        setHallSeats(hallWithDetails[0].seats);
      }
    }, [hallWithDetails]);

    const changeSeatType = (
      seatNumber: number,
      seatType?: SeatType,
      maxNumberOfColumns?: number
    ) => {
      if (maxNumberOfColumns != undefined) {
        setHallSeats((prevSeats) => {
          // Seat not exist or exisst, seatType is undefined
          if (seatType === undefined) {
            return prevSeats.filter((seat) => seat.seatNr !== seatNumber);
          }

          // Seat exist, seatType is defined
          const seatExists = prevSeats.some((seat) => seat.seatNr === seatNumber);
          if (seatExists) {
            return prevSeats.map((seat) => {
              if (seat.seatNr === seatNumber) {
                return {
                  ...seat,
                  seatType: seatType,
                };
              }
              return seat;
            });
          }

          // Seat not exist, seatType is defined
          return [
            ...prevSeats,
            {
              seatNr: seatNumber,
              row: Math.ceil(seatNumber / maxNumberOfColumns),
              gridRow: Math.ceil(seatNumber / maxNumberOfColumns),
              column: ((seatNumber - 1) % maxNumberOfColumns) + 1,
              isAvailable: true,
              gridColumn: ((seatNumber - 1) % maxNumberOfColumns) + 1,
              seatType: seatType,
            } as Seat,
          ];
        });
      }
    };

    const generateContextMenuItems = () => {
      if (!seatTypes) return [];

      const items = seatTypes.map((seatType) => ({
        label: seatType.name,
        action: (selectedSeatNr?: number) => {
          if (selectedSeatNr !== undefined)
            changeSeatType(
              selectedSeatNr,
              seatType,
              hallWithDetails[0].maxNumberOfSeatsColumns
            );
        },
      }));

      items.push({
        label: "Miejsce nieaktywne",
        action: (selectedSeatNr?: number) => {
          if (selectedSeatNr !== undefined)
            changeSeatType(
              selectedSeatNr,
              undefined,
              hallWithDetails[0].maxNumberOfSeatsColumns
            );
        },
      });

      return items;
    };

    const getSeats = (maxNumberOfSeatsRows?: number, maxNumberOfSeatsColumns?: number) => {
      if (maxNumberOfSeatsRows !== undefined && maxNumberOfSeatsColumns !== undefined) {
        return (
          <div className="flex flex-col gap-[10px]" onContextMenu={(e) => e.preventDefault()}>
            {Array.from({ length: maxNumberOfSeatsRows }, (_, rowIndex) => (
              <div
                key={rowIndex}
                className="grid gap-[10px]"
                style={{
                  gridTemplateColumns: `repeat(${maxNumberOfSeatsColumns}, minmax(0, 1fr))`,
                }}
              >
                {Array.from({ length: maxNumberOfSeatsColumns }, (_, colIndex) => {
                  const seatNumber = rowIndex * maxNumberOfSeatsColumns + colIndex + 1;
                  const seat = hallSeats.find((s) => s.seatNr == seatNumber);
                  //const isAvailable = seatNumbers.includes(seatNumber);

                  return (
                    <div
                      key={`${rowIndex}-${colIndex}`}
                      className={`border px-[8px] py-[4px] text-center text-[12px] flex flex-col justify-center gap-[2px] items-center ${
                        seat
                          ? "border-gray-300 text-white hover:cursor-pointer"
                          : "border-gray-400 bg-gray-200 text-gray-500"
                      }`}
                      // onContextMenu={(event) => onRightClick(event, seatNumber)}
                      onContextMenu={(e) =>
                        handleContextMenu(
                          e,
                          seatNumber,
                          maxNumberOfSeatsRows,
                          maxNumberOfSeatsColumns
                        )
                      }
                      style={{ backgroundColor: seat?.seatType?.seatColor }}
                    >
                      <FontAwesomeIcon icon={faCouch} fontSize={12} />
                      <p className="hover:cursor-default text-[11px]">{seatNumber}</p>
                    </div>
                  );
                })}
              </div>
            ))}
          </div>
        );
      }
    };

    const handleContextMenu = (
      event: React.MouseEvent,
      seatNumber: number,
      _rows: number,
      cols: number
    ) => {
      event.preventDefault();
      //setSelectedSeatNr(seatNumber);
      const x = 70 + ((seatNumber - 1) % cols) * 48;
      const y = 140 + Math.floor((seatNumber - 1) / cols) * 45;
      setContextMenu({
        selectedSeatNr: seatNumber,
        position: { x: x, y: y },
        items: generateContextMenuItems(),
      });
    };

    const handleCloseContextMenu = () => {
      setContextMenu(null);
    };

    const seatAsRequest = (seat: Seat, maxNumberOfColumns: number): SeatRequest => {
      const seatRequest = {
        seatNr: seat.seatNr,
        row: Math.ceil(seat.seatNr / maxNumberOfColumns),
        gridRow: Math.ceil(seat.seatNr / maxNumberOfColumns),
        column: ((seat.seatNr - 1) % maxNumberOfColumns) + 1,
        gridColumn: ((seat.seatNr - 1) % maxNumberOfColumns) + 1,
        seatTypeId: seat.seatType?.id,
      } as SeatRequest;

      const parsedSeatRequest = SeatSchema.safeParse(seatRequest);

      if (!parsedSeatRequest.success) {
        const errors = parsedSeatRequest.error.errors
          .map((err) => `${err.path.join(".")}: ${err.message}`)
          .join(", ");
        throw new Error(errors);
      }

      return parsedSeatRequest.data;
    };

    const onModifyButtonClick = async (maxNumberOfColumns?: number) => {
      if (maxNumberOfColumns != undefined) {
        const requestSeats: SeatRequest[] = [];
        hallSeats.forEach((seat) => {
          try {
            const validatedSeat = seatAsRequest(seat, maxNumberOfColumns);
            requestSeats.push(validatedSeat);
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : "Nieznany błąd.";
            toast.error(errorMessage);
            return;
          }
        });

        const requestHallModify = {
          seats: requestSeats,
        } as HallRentHallUpdateRequest;

        console.log(requestSeats);
        setPromisePending(true);
        await toast.promise(updateHall({ id: hallRentId, body: requestHallModify }), {
          pending: "Wykonywanie żądania",
          success: "Sala została zaktualizowana pomyślnie",
          error: "Wystąpił błąd podczas aktualizacji sali",
        });
        setPromisePending(false);
        setActionPerformed(true);
      }
    };

    useEffect(() => {
      if (actionPerformed) {
        if (statusCode == HTTPStatusCode.NoContent) {
          onDialogConfirm();
        }
        setActionPerformed(false);
      }
    }, [actionPerformed]);

    return (
      <div>
        {hallId !== undefined && (
          <Dialog ref={ref} minHeight={600} maxHeight={1200} marginTop={20}>
            <div className="flex flex-row justify-center items-start gap-3">
              <div className="flex flex-col justify-center items-center px-5 pb-2 gap-6">
                <div className="flex flex-col justify-center items-center gap-2">
                  <h2>Modyfikacja sali</h2>
                  <p className="text-textPrimary text-base text-center">
                    Zmodyfikuj salę zmieniając ustawienie lub typ miejsc
                  </p>
                </div>
                <div className="flex flex-col justify-center items-center gap-3">
                  {/* <div>stage</div> */}
                  <div className="flex flex-col gap-2">
                    {getSeats(
                      hallWithDetails[0]?.maxNumberOfSeatsRows,
                      hallWithDetails[0]?.maxNumberOfSeatsColumns
                    )}
                    {contextMenu && (
                      <ContextMenu
                        selectedSeatNr={contextMenu.selectedSeatNr}
                        items={contextMenu.items}
                        position={contextMenu.position}
                        onClose={handleCloseContextMenu}
                      />
                    )}
                  </div>
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
                    text={promisePending ? "Ładowanie..." : "Modyfikuj"}
                    width={145}
                    height={45}
                    icon={faPenToSquare}
                    iconSize={18}
                    style={ButtonStyle.Primary}
                    action={() =>
                      onModifyButtonClick(hallWithDetails[0].maxNumberOfSeatsColumns)
                    }
                  ></Button>
                </div>
              </div>
              <div className="self-stretch min-w-[1px] bg-black"></div>
              <div className="pl-3 flex flex-col justify-centers items-start">
                <article className="flex flex-col justify-start items-start gap-1 w-full">
                  <h5 className="text-center font-semibold text-base w-full">Typy miejsc:</h5>
                  <div className="flex justify-start flex-col gap-2">
                    {seatTypes.map((type, index) => (
                      <div key={index} className="flex flex-row justify-start items-center gap-2">
                        <div
                          className="py-[2px] px-[6px]"
                          style={{ backgroundColor: type.seatColor }}
                        >
                          <FontAwesomeIcon
                            fontSize={12}
                            icon={faCouch}
                            color="white"
                          ></FontAwesomeIcon>
                        </div>
                        <p className="text-[14px]">{type.name}</p>
                      </div>
                    ))}
                    <div className="flex flex-row justify-start items-center gap-2">
                      <div className="py-[2px] px-[6px] border-gray-400 bg-gray-200">
                        <FontAwesomeIcon
                          fontSize={12}
                          icon={faCouch}
                          color="#6b7280"
                        ></FontAwesomeIcon>
                      </div>
                      <p className="text-[14px]">Miejsce nieaktywne</p>
                    </div>
                  </div>
                </article>
                <article className="self-start flex flex-col justify-center items-center gap-2 mt-2 w-full">
                  <h5 className="font-semibold text-base w-full text-center">Informacje o sali:</h5>
                  <LabelText
                    fontSize={14}
                    labelWidth={120}
                    label="Nr sali:"
                    isTextLeft={true}
                    text={hallWithDetails[0]?.hallNr}
                  />
                  <LabelText
                    fontSize={14}
                    labelWidth={120}
                    isTextLeft={true}
                    label="Piętro:"
                    text={hallWithDetails[0]?.floor}
                  />
                  <LabelText
                    fontSize={14}
                    labelWidth={120}
                    isTextLeft={true}
                    label="Typ:"
                    text={hallWithDetails[0]?.type?.name}
                  />{" "}
                  <LabelText
                    fontSize={14}
                    labelWidth={120}
                    isTextLeft={true}
                    label="Pow sali (m2):"
                    text={hallWithDetails[0]?.totalArea}
                  />
                  <LabelText
                    fontSize={14}
                    labelWidth={120}
                    isTextLeft={true}
                    label="Pow sceny (m2):"
                    text={hallWithDetails[0]?.stageArea}
                  />
                  <LabelText
                    fontSize={14}
                    labelWidth={120}
                    isTextLeft={true}
                    label="Ilość miejsc:"
                    text={hallSeats.length}
                  />
                  <LabelText
                    fontSize={14}
                    labelWidth={120}
                    isTextLeft={true}
                    label="Max ilość miejsc:"
                    text={hallWithDetails[0]?.maxNumberOfSeats}
                  />
                </article>
                <article className="self-start flex flex-col justify-center items-center mt-4 w-full">
                  <MessageText
                    maxWidth={330}
                    messageType={MessageType.Info}
                    fontSize={14}
                    iconSize={14}
                    text={`Aby zmienić typ miejsca lub ustawić je jako nieaktywne kliknij prawym przyciskiem myszy lub przytrzymaj na ikonie wybranego miejsca, pojawi się wtedy menu kontekstowe z którego możesz wybrać odpowiedni typ miejsca.`}
                  />
                </article>
              </div>
            </div>
          </Dialog>
        )}
      </div>
    );
  }
);

export default ModifyHallDialog;
