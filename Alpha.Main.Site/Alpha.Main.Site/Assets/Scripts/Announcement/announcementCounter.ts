module PopupAnnouncement {
    interface IAnnouncementCounter {
        Id: string;
        Count: number;
    }

    export class AnnouncementCounter {
        protected $: JQueryStatic;
        protected maximun = 5;
        protected name: string;
        protected announcements: Array<IAnnouncementCounter>;

        constructor($: JQueryStatic) {
            this.$ = $;
            this.announcements = [];
        }

        init = (name: string) => {
            this.name = name;

            var object = localStorage.getItem(this.name);
            if (object != null) {
                this.announcements = JSON.parse(object);
            }
        }

        increaseCounter = (id: string) => {
            let existed = false;
            for (let item of this.announcements) {
                if (item.Id == id) {
                    item.Count++;
                    existed = true;
                }
            }

            if (!existed) {
                this.announcements.push({ Id: id, Count: 1 })
            }

            this.save();
        }

        isExccess = (id: string) => {
            var count = 0;
            for (let item of this.announcements) {
                if (item.Id == id) {
                    count = item.Count;
                }
            }

            return count >= this.maximun;
        }

        private save = () => {
            var objectString = JSON.stringify(this.announcements);
            localStorage.setItem(this.name, objectString);
        }
    }
}