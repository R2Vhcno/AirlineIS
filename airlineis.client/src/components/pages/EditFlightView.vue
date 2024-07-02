<script setup>
    import { ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import { fetchDelete, fetchGet, fetchGetId, fetchPost, fetchPut } from '@/fetchutils.js'

    let flightShouldBeCreated = false;

    let route = useRoute();
    let router = useRouter();

    let airports = ref(null);
    let planes = ref(null);

    let selectedOrigin = ref(0);
    let selectedDestination = ref(0);
    let selectedPlane = ref(0);
    let departureDate = ref("");
    let flightDuration = ref("");

    watch(() => route.params.flightid, (newId, oldId) => {
        initForm();
    });

    async function initForm() {
        let id = new Number(route.params.flightid);

        await fetchGet('/api/airports', {
            success: (response, body) => {
                airports.value = body;
            }
        });

        await fetchGet('/api/planes', {
            success: (response, body) => {
                planes.value = body;
            }
        });

        if (id == 0) {
            flightShouldBeCreated = true;

            selectedOrigin.value = airports.value[0].id;
            selectedDestination.value = airports.value[0].id;
            selectedPlane.value = planes.value[0].id;

            return;
        }

        await fetchGetId('/api/flights', id, {
            success: (response, body) => {
                selectedOrigin.value = body.originId;
                selectedDestination.value = body.destinationId;
                selectedPlane.value = body.planeId;
                departureDate.value = body.departureTime;
                flightDuration.value = body.duration;

            }
        });
    }

    async function commitChanges() {
        let id = new Number(route.params.flightid);

        let flight = {
            id: null,
            originId: selectedOrigin.value,
            destinationId: selectedDestination.value,
            planeId: selectedPlane.value,
            departureTime: "",
            duration: flightDuration.value,
        }

        flight.departureTime = new Date(departureDate.value).toISOString();

        if (flight.duration.split(':').length != 3) {
            flight.duration += ':00';
        }

        if (flightShouldBeCreated) {
            await fetchPost('/api/flights', flight, {
                success: (r, b) => {
                    router.push({ path: '/' });
                }
            });

            return;
        }

        flight.id = id;

        await fetchPut('/api/flights', id, flight, {
            success: (r, b) => {
                router.push({ path: '/' });
            },
            error: (r, b) => {
                alert(r.status);
                alert(JSON.stringify(flight));
            }
        });
    }

    initForm();
</script>

<template>
    <h2>Редактирование рейса</h2>

    <div class="form">
        <div class="form form-group">
            <h3>Маршрут</h3>

            <label for="origin-select">Аэропорт отправления:</label>
            <select id="origin-select" v-model="selectedOrigin">
                <option v-for="airport in airports" :value="airport.id">
                    {{ airport.name }} ({{ airport.code }})
                </option>
            </select>

            <label for="destination-select">Аэропорт прибытия:</label>
            <select id="destination-select" v-model="selectedDestination">
                <option v-for="airport in airports" :value="airport.id">
                    {{ airport.name }} ({{ airport.code }})
                </option>
            </select>
        </div>

        <div class="v-spacer"></div>

        <label for="plane-select">Самолёт:</label>
        <select id="plane-select" v-model="selectedPlane">
            <option v-for="plane in planes" :value="plane.id">
                {{ plane.name }}
            </option>
        </select>

        <div class="v-spacer"></div>
        
        <label for="departure-select">Дата отправления:</label>
        <input v-model="departureDate" id="departure-select" type="datetime-local">

        <label for="duration-select">Длительность перелёта:</label>
        <input v-model="flightDuration" id="duration-select" type="time">

        <div>
            <button @click="commitChanges()">{{ flightShouldBeCreated ? "Создать" : "Изменить" }}</button>
            <button @click="$router.push({ path: '/'})">Отмена</button>
        </div>
    </div>
</template>

<style scoped>
</style>