import "./styles/quasar.scss";
import "@quasar/extras/material-icons/material-icons.css";
import { Notify } from "quasar";

export default {
  config: {
    notify: {
      group: true,
      timeout: 10000,
      closeBtn: "Close",
      multiLine: true,
      progress: true,
      classes: "user-notification",
      progressClass: "user-notification-progress",
    },
  },
  plugins: { Notify }
}