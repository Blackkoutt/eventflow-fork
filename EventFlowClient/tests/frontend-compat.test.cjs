const assert = require("node:assert/strict");
const fs = require("node:fs");
const test = require("node:test");
const ts = require("typescript");

// Load the application's small TypeScript helpers without a second test framework.
require.extensions[".ts"] = (module, filename) => {
  const { outputText } = ts.transpileModule(fs.readFileSync(filename, "utf8"), {
    compilerOptions: {
      module: ts.ModuleKind.CommonJS,
      target: ts.ScriptTarget.ES2020,
      esModuleInterop: true,
    },
    fileName: filename,
  });
  module._compile(outputText, filename);
};

const { getUserFormCookie } = require("../src/helpers/cookies/JWTCookie.ts");
const { isUserInRole } = require("../src/models/response_models/User.ts");
const { Roles } = require("../src/helpers/enums/UserRoleEnum.ts");
const DateFormatter = require("../src/helpers/DateFormatter.ts").default;

test("JWT restoration uses API emailAddress and accepts identity without profile statistics", () => {
  const claims = {
    id: "test-user",
    name: "Test",
    surname: "User",
    emailAddress: "test@example.invalid",
    dateOfBirth: "2000-01-01",
    userRoles: "Admin",
  };
  const token = `${Buffer.from('{"alg":"none"}').toString("base64url")}.${Buffer.from(JSON.stringify(claims)).toString("base64url")}.`;
  global.document = { cookie: `EventFlowJWTCookie=${token}` };
  try {
    assert.deepEqual(getUserFormCookie(), claims);
    document.cookie = "";
    assert.equal(getUserFormCookie(), null);
  } finally {
    delete global.document;
  }
});

test("role checks accept single JWT role, role arrays, and unauthenticated users", () => {
  assert.equal(isUserInRole({ userRoles: "Admin" }, Roles.Admin), true);
  assert.equal(isUserInRole({ userRoles: ["User", "Admin"] }, Roles.Admin), true);
  assert.equal(isUserInRole({ userRoles: "User" }, Roles.Admin), false);
  assert.equal(isUserInRole(null, Roles.Admin), false);
});

test("installed Schedule-X supports date formatting, plugin updates, and clearing stale events", () => {
  // Calendar creation reads HTML direction; rendering and browser layout are outside this test.
  global.document = { querySelector: () => null };
  const { createCalendar, createViewMonthGrid } = require("@schedule-x/calendar");
  const { createEventsServicePlugin } = require("@schedule-x/events-service");
  const eventsService = createEventsServicePlugin();
  const start = DateFormatter.FormatDateForCalendar("2030-01-02T10:00:00");
  const end = DateFormatter.FormatDateForCalendar("2030-01-02T11:00:00");
  const calendar = createCalendar({
    views: [createViewMonthGrid()],
    selectedDate: start.split(" ")[0],
    plugins: [eventsService],
  });
  try {
    eventsService.set([{ id: 1, title: "Event without hall rents", start, end }]);
    assert.equal(eventsService.getAll()[0].title, "Event without hall rents");
    assert.equal(eventsService.getAll()[0].start, "2030-01-02 10:00");
    eventsService.set([]);
    assert.deepEqual(eventsService.getAll(), []);
  } finally {
    calendar.destroy();
    delete global.document;
  }
});
