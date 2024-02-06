<template>
    <div v-if="loading">
        <p>Loading...</p>
    </div>

    <div v-if="todoItems">
        <table>
            <thead>
                <tr>
                    <th>id</th>
                    <th>name</th>
                    <th>is complete</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="todo in todoItems" :key="todo.id">
                    <td>{{ todo.id }}</td>
                    <td>{{ todo.name }}</td>
                    <td>{{ todo.isComplete }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type TodoList = {
        id: number,
        name: string,
        isComplete: boolean
    }[];

    interface Data {
        loading: boolean,
        todoItems: null | TodoList
    }

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                todoItems: null
            };
        },
        created() {
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData(): void {
                this.loading = true;
                this.todoItems = null;
                console.log("fetchData");

                fetch('https://localhost:7174/api/TodoItems', {
                    //mode: 'no-cors'
                })
                    .then(r => r.json())
                    .then(json => {
                        console.log(json);
                        this.todoItems = json as TodoList;
                        this.loading = false;
                        return;
                    });
            }
        },
    });
</script>

<style scoped lang="scss">
    $pad: .5rem;

    th {
        font-weight: bold;
    }

/*    tr:nth-child(even) {
        background: #F2F2F2;
    }

    tr:nth-child(odd) {
        background: #FFF;
    }*/

    th, td {
        padding-left: $pad;
        padding-right: $pad;
    }

    .weather-component {
        text-align: center;
    }

    table {
        margin-left: auto;
        margin-right: auto;
    }
</style>


<style>
</style>